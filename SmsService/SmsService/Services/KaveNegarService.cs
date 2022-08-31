using ErrorHandlingDll.FixTypes.Enumarions;
using ErrorHandlingDll.Interfaces;
using ErrorHandlingDll.ReturnTypes;
using Kavenegar;
using Kavenegar.Exceptions;
using Kavenegar.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoRepository.Repository;
using SmsService.Dtos.Sms;
using SmsService.Entities;
using SmsService.Interfaces;
using SmsService.Mappers;
using System.Net;
using ErrorHandlingDll.FixTypes.Enumarions;
using LogLevel = ErrorHandlingDll.FixTypes.Enumarions.LogLevel;
using static SmsService.Percistance.BaseData;
using SmsService.Percistance;

namespace SmsService.Services
{
  public class KaveNegarService  : ISendSmsService
  {
    private readonly AppSetting _appSetting;
    private readonly ISmsService _smsService;
    private KavenegarApi _kaveNegarApi;
    private readonly ILoggerService _loggerService;
    public KaveNegarService(ISmsService smsService , IOptions<AppSetting> appSetting , Func<LoggerIds, ILoggerService> loggerService)
    {
      _smsService = smsService;
      _appSetting = appSetting.Value;
      _kaveNegarApi = new KavenegarApi(_appSetting.KaveNegar.ApiKey);
      _loggerService = loggerService(LoggerIds.Sentry);

    }
    public async Task<ReturnModel<SendSmsReturnDto>> SendSmsAsync(SmsInputDto sendSmsInputDto)
    {
      ReturnModel<SendSmsReturnDto> result = new();
      SmsModel smsModel = new();
      smsModel.CreateNewSmsModel(sendSmsInputDto);

      var createNewSms = await _smsService.CreateSmsAsync(smsModel);
      if(createNewSms.HttpStatusCode is not HttpStatusCode.OK || createNewSms.Data is null)
      {
        result.CreateServerErrorModel();
        return result;
      }

      SmsModel newSms = createNewSms.Data;

      (bool isSuccessFull, SendResult result ,string message) sendSms = await SendByKaveNegarAsync(newSms);
   
      if (sendSms.isSuccessFull)
      {
        await UpdateSmsStatus(newSms, sendSms.result, isSent:true);
        SendSmsReturnDto sendSmsReturnModel = new(newSms.Id.ToString());
        result.CreateSuccessModel(data: sendSmsReturnModel, title: "SMSId") ;
        return result;
      }     
      else
      {
        await UpdateSmsStatus(newSms, sendSms.result, isSent: false);
        result.CreateServerErrorModel(message: $"{sendSms.message}");
        return result;
      }
        
    }
    public async Task<ReturnModel<SendSmsReturnDto>> SendLoginCode(LoginSmsInputDto loginSmsInputDto)
    {
      ReturnModel<SendSmsReturnDto> result = new();
      SmsModel smsModel = new();
      smsModel.CreateLoginSmsModel(loginSmsInputDto);

      var createNewSms = await _smsService.CreateSmsAsync(smsModel);
      if (createNewSms.HttpStatusCode is not HttpStatusCode.OK || createNewSms.Data is null)
      {
        result.CreateServerErrorModel();
        return result;
      }

      SmsModel newSms = createNewSms.Data;

      (bool isSuccessFull, SendResult result, string message) sendSms = await SendLookUpByKaveNegar(newSms.Receiever.PhoneNumber,loginSmsInputDto.code, loginSmsInputDto.appName);

      if (sendSms.isSuccessFull)
      {
        await UpdateSmsStatus(newSms, sendSms.result, isSent: true);
        SendSmsReturnDto sendSmsReturnModel = new(newSms.Id.ToString());
        result.CreateSuccessModel(data: sendSmsReturnModel, title: "SMSId");
        return result;
      }
      else
      {
        await UpdateSmsStatus(newSms, sendSms.result,isSent:false);
        result.CreateServerErrorModel(message: $"{sendSms.message}");
        return result;
      }
    }
    public async Task<StatusResult> CheckSmsDelivery(long messageId)
    {
      StatusResult status = _kaveNegarApi.Status(new List<string>() { messageId.ToString() })?.FirstOrDefault();
      ReturnModel<SmsModel> sms = await _smsService.GetSmsByMessageIdAsync(messageId);

      await UpdateDeliveryStatus(sms.Data ,status, true);
      return status;

    }
    private async Task<(bool isSuccessFull , SendResult , string errorMessage)> SendByKaveNegarAsync(SmsModel sms)
    {
      try
      {
        SendResult sendSms =
       _kaveNegarApi.Send(_appSetting.KaveNegar.Sender,
                         sms.Receiever.PhoneNumber,
                         sms.Content);

        return (true, sendSms, sendSms.StatusText);
      }
      catch (ApiException ex)
      {
        //if the http response of web service is not 200 this exception will rise
        await _loggerService.CaptureLogAsync(LogLevel.Error, ex);
          return (false, null , ex.Message);

      }
      catch (HttpException ex)
      {
        //if the service is not reachable this exception will rise
        return (false, null, ex.Message);
      }      
    }
    private async Task<(bool isSuccessFull, SendResult, string errorMessage)> SendLookUpByKaveNegar(string reciever, string token,string token2)
    {
      try
      {
        SendResult sendSms = _kaveNegarApi.VerifyLookup(reciever,token,token2:token2 ,null,KaveNegarLookupTemplates.OPT);
        return (true, sendSms, sendSms.StatusText);
      }
      catch (ApiException ex)
      {
        //if the http response of web service is not 200 this exception will rise
        await _loggerService.CaptureLogAsync(LogLevel.Error, ex);
        return (false, null, ex.Message);

      }
      catch (HttpException ex)
      {
        //if the service is not reachable this exception will rise
        return (false, null, ex.Message);
      }
    }
    private async Task UpdateSmsStatus(SmsModel sms , SendResult sendResult, bool isSent)
    {
      SmsModel updatedSmsModel = sms.UpdateSmsStatus(sendResult,isSent: isSent);
      await _smsService.UpdateSms(updatedSmsModel);
    }
    private async Task UpdateDeliveryStatus(SmsModel sms, StatusResult statusResult, bool isDelivered)
    {
      SmsModel updatedSmsModel = sms.UpdateSmsDeliveryStatus(statusResult, isDelivered);
      await _smsService.UpdateSms(updatedSmsModel);
    }

  }
}
