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

      var createNewSms = await _smsService.CreateSmsAsync(sendSmsInputDto);
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
        await FailSms(newSms,sendSms.message);
        result.CreateServerErrorModel(message: $"{sendSms.message}");
        return result;
      }
        
    }

    private async Task<(bool isSuccessFull , SendResult , string errorMessage)> SendByKaveNegarAsync(SmsModel sms)
    {


      try
      {
        SendResult sendSms =
       _kaveNegarApi.Send(_appSetting.KaveNegar.Sender,
                         sms.Receiver.PhoneNumber,
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

    private async Task FailSms(SmsModel sms ,string message)
    {
      UpdateSmsDto updateSmsModel = SmsMappers.CreateFailedUpdateModel(message);
      await _smsService.UpdateSms(sms,updateSmsModel);
    }

    private async Task SucceedSms(SmsModel sms, SendResult sendResult)
    {
      UpdateSmsDto updateSmsModel = SmsMappers.CreateSuccessedUpdateModel(sendResult.Cost,
        sendResult.Status,
        sendResult.StatusText, sendResult.Messageid, sendResult.Message,
        sendResult.Date, sendResult.GregorianDate);

      await _smsService.UpdateSms(sms, updateSmsModel);
    }

  }
}
