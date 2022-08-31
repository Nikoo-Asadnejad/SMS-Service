using ErrorHandlingDll.ReturnTypes;
using SmsService.Dtos.Sms;
using SmsService.Entities;

namespace SmsService.Interfaces
{
  public interface ISmsService
  {
    Task<ReturnModel<SmsModel>> CreateSmsAsync(SmsModel sendSmsInputDto);

    Task<ReturnModel<SmsReturnDto>> GetSmsAsync(string id);

    Task<ReturnModel<SmsModel>> GetSmsByMessageIdAsync (long messageId);  

    Task<bool> UpdateSms(SmsModel newSms);

  }
}
