using MongoDB.Bson.Serialization.Attributes;
using SmsService.Mappers;

namespace SmsService.Entities
{
  public class Provider
  {
    [BsonRequired]
    public int Id { get; set; }
    [BsonRequired]
    public string Name { get; set; }
    public string Number { get; set; }
    public Provider(int id, string name,string number)
    {
      Id = id;
      Name = name;
      Number = number;
    }

    public Provider(int id, string number)
    {
      Id =id;
      Name = BaseDataMappers.GetProviderById(id);
      Number = number;
    }
    public Provider()
    {

    }

    

   

  }
}
