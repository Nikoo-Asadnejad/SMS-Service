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

namespace SmsService.Services
{
  public class KaveNegarService  : ISendSmsService
  {
    private readonly AppSetting _appSetting;
    private readonly ISmsService _smsService;
    private KavenegarApi _kaveNegarApi;

   // private readonly ILoggerService _loggerService;
    public KaveNegarService(ISmsService smsService , IOptions<AppSetting> appSetting )
    {
      _smsService = smsService;
      _appSetting = appSetting.Value;
      _kaveNegarApi = new KavenegarApi(_appSetting.KaveNegar.ApiKey);
     // _loggerService = loggerService;
    }

    public async Task<ReturnModel<string>> SendSmsAsync(SmsInputDto sendSmsInputDto)
    {
      ReturnModel<string> result = new();

      var createNewSmsResult = await _smsService.CreateSmsAsync(sendSmsInputDto);
      if(createNewSmsResult.HttpStatusCode is not HttpStatusCode.OK || createNewSmsResult.Data is null)
      {
        result.CreateServerErrorModel();
        return result;
      }

      SmsModel newSms = createNewSmsResult.Data;

      (bool isSuccessFull, SendResult result ,string message) sendSms = await SendByKaveNegarAsync(newSms);

     
      if (sendSms.isSuccessFull)
      {
        await SucceedSms(newSms, sendSms.result);
        result.CreateSuccessModel(data: newSms.Id.ToString());
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
        // در صورتی که خروجی وب سرویس 200 نباشد این خطارخ می دهد.
          return (false, null , ex.Message);
      }
      catch (HttpException ex)
      {
        // در زمانی که مشکلی در برقرای ارتباط با وب سرویس وجود داشته باشد این خطا رخ می دهد
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
