using MongoDB.Bson.Serialization.Attributes;

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

    

   

  }
}
