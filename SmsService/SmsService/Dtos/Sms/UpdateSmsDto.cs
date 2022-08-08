using SmsService.Entities;

namespace SmsService.Dtos.Sms
{
  public record UpdateSmsDto(bool? isSuccessful ,
                            int? Cost,
                            SendingStatusModel SendingStatus ,
                            ProviderResultModel ProviderResult );
}
