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
        Providers.Najva.Id => Providers.Najva.Name,
        _ => Providers.Najva.Name
      };
  }
}
