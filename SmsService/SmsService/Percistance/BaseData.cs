using MongoDB.Bson;

namespace SmsService.Percistance
{
  public struct BaseData
  {
    public struct Providers
    {
      public struct KaveNegar
      {
        public const int Id = 1;
        public const string Name = "KaveNegar";
        public const string PhoneNumber = "10008663";
      }

    }

    public struct SmsTypes
    {
      public struct OPT
      {
        public const int Id = 2;
        public const string Name = "OPT";
      }

    }

  }

}
