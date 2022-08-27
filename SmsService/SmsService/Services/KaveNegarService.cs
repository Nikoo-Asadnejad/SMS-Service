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
      SmsModel smsModel = sendSmsInputDto.CreateSmsModel();

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
        await SucceedSms(newSms, sendSms.result);
        SendSmsReturnDto sendSmsReturnModel = new(newSms.Id.ToString());
        result.CreateSuccessModel(data: sendSmsReturnModel, title: "SMSId") ;
        return result;
      }     
      else
      {
        await FailSms(newSms,sendSms.result);
        result.CreateServerErrorModel(message: $"{sendSms.message}");
        return result;
      }
        
    }
    public async Task<ReturnModel<SendSmsReturnDto>> SendLoginCode(LoginSmsInputDto loginSmsInputDto)
    {
      ReturnModel<SendSmsReturnDto> result = new();
      SmsModel smsModel = loginSmsInputDto.CreateLoginSmsModel();

      var createNewSms = await _smsService.CreateSmsAsync(smsModel);
      if (createNewSms.HttpStatusCode is not HttpStatusCode.OK || createNewSms.Data is null)
      {
        result.CreateServerErrorModel();
        return result;
      }

      SmsModel newSms = createNewSms.Data;

      (bool isSuccessFull, SendResult result, string message) sendSms = await SendLookUpByKaveNegar(newSms.Receiever.PhoneNumber,loginSmsInputDto.code, SmsMessagesTemplates.LoginSmsTemplate);

      if (sendSms.isSuccessFull)
      {
        await SucceedSms(newSms, sendSms.result);
        SendSmsReturnDto sendSmsReturnModel = new(newSms.Id.ToString());
        result.CreateSuccessModel(data: sendSmsReturnModel, title: "SMSId");
        return result;
      }
      else
      {
        await FailSms(newSms, sendSms.result);
        result.CreateServerErrorModel(message: $"{sendSms.message}");
        return result;
      }
    }
    public async Task<StatusResult> CheckSmsDelivery(long messageId)
    {
      StatusResult status = _kaveNegarApi.Status(new List<string>() { messageId.ToString() })?.FirstOrDefault();
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
    private async Task<(bool isSuccessFull, SendResult, string errorMessage)> SendLookUpByKaveNegar(string reciever, string token , string template)
    {
      try
      {
        SendResult sendSms = _kaveNegarApi.VerifyLookup(reciever,token,template);
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
    private async Task FailSms(SmsModel sms , SendResult sendResult)
    {
      UpdateSmsDto updateSmsModel = SmsMappers.CreateNotSentModel(sendResult);
      await _smsService.UpdateSms(sms,updateSmsModel);
    }
    private async Task SucceedSms(SmsModel sms, SendResult sendResult)
    {
      UpdateSmsDto updateSmsModel = SmsMappers.CreateSentModel(sendResult);
      await _smsService.UpdateSms(sms, updateSmsModel);
    }


    //private async Task UpdateDeliveryStatus(SmsModel sms ,StatusResult statusResult)
    //{
    //  UpdateSmsDto updateSmsModel = SmsMappers.CreateSentModel(sendResult);
    //  await _smsService.UpdateSms(sms, updateSmsModel);
    //}

  }
}
