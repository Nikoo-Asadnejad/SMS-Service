using MongoDB.Bson;

namespace SmsService.Percistance
{
  public struct BaseData
  {
    public struct Providers
    {
      public struct Najva
      {
        public const int Id = 1;
        public const string Name = "Najva";
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
