using ErrorHandlingDll.ReturnTypes;
using SmsService.Dtos.Sms;
using SmsService.Entities;

namespace SmsService.Interfaces
{
  public interface ISmsService
  {
    Task<ReturnModel<SmsModel>> CreateSmsAsync(SmsInputDto sendSmsInputDto);

    Task<ReturnModel<SmsReturnDto>> GetSmsAsync(string id);

    Task<bool> UpdateSms(SmsModel sms, UpdateSmsDto updateSmsDto);

  }
}
