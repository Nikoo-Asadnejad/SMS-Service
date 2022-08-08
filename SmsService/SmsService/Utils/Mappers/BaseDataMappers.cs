using Microsoft.Extensions.Options;
using static SmsService.Percistance.BaseData;

namespace SmsService.Mappers
{
  public static class BaseDataMappers
  {

    public static string GetSmsTypeById(int typeId)
      => typeId switch
      {
        SmsTypes.OPT.Id => SmsTypes.OPT.Name,
        _ => SmsTypes.OPT.Name
      };

    public static string GetProviderById(int providerId)
      => providerId switch
      {
        Providers.KaveNegar.Id => Providers.KaveNegar.Name,
        _ => Providers.KaveNegar.Name
      };

    public static string GetSenderPhoneByProviderId(int providerId)
      => providerId switch
      {
        Providers.KaveNegar.Id => Providers.KaveNegar.PhoneNumber,
        _ => Providers.KaveNegar.PhoneNumber
      };
  }
}
