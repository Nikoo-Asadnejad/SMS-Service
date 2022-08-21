using SmsService.Entities;

namespace SmsService.Dtos.Sms
{
  public record UpdateSmsDto(bool IsSent ,
                            int? Cost,
                            int? Status,
                            string StatusMessage,
                            long? messageId);
}
