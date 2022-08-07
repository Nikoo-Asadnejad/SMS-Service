using ErrorHandlingDll.ReturnTypes;
using SmsService.Dtos.Sms;

namespace SmsService.Interfaces
{
  public interface ISmsService
  {
    Task<ReturnModel<string>> CreateSmsAsync(SmsInputDto sendSmsInputDto);

    Task<ReturnModel<SmsReturnDto>> GetSmsAsync(string id);

  }
}
