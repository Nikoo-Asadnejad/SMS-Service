using ErrorHandlingDll.ReturnTypes;
using SmsService.Dtos.Sms;

namespace SmsService.Interfaces
{
  public interface ISendSmsService
  {
    Task<ReturnModel<SendSmsReturnDto>> SendSmsAsync(SmsInputDto sendSmsInputDto);
  }
}
