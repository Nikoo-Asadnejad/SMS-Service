using ErrorHandlingDll.Interfaces;
using ErrorHandlingDll.ReturnTypes;
using MongoDB.Bson;
using MongoRepository.Repository;
using SmsService.Dtos.Sms;
using SmsService.Entities;
using SmsService.Interfaces;
using SmsService.Mappers;
using System.Net;

namespace SmsService.Services
{
  public class NajvaSmsService  :ISendSmsService
  {

    private readonly ISmsService _smsService;
   // private readonly ILoggerService _loggerService;
    public NajvaSmsService( ISmsService smsService )
    {
      _smsService = smsService;
     // _loggerService = loggerService;
    }

    public async Task<ReturnModel<string>> SendSmsAsync(SmsInputDto sendSmsInputDto)
    {
      ReturnModel<string> result = new();

      var createNewSms = await _smsService.CreateSmsAsync(sendSmsInputDto);
      if(createNewSms.HttpStatusCode is not HttpStatusCode.OK || createNewSms.Data is null)
      {
        result.CreateServerErrorModel();
        return result;
      }

      string smsId = createNewSms.Data;
      result.CreateSuccessModel(data: smsId);
      return result;
    }


  }
}
