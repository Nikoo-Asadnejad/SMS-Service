using ErrorHandlingDll.ReturnTypes;
using SmsService.Dtos.Sms;

namespace SmsService.Interfaces
{
  public interface ISendSmsService
  {
    Task<ReturnModel<string>> SendSmsAsync(SmsInputDto sendSmsInputDto);
  }
}
