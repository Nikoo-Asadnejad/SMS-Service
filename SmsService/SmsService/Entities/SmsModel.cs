using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoRepository.Atrributes;
using MongoRepository.Models;

namespace SmsService.Entities
{

  [MongoCollectionAttribute("Sms")]
  public class SmsModel : MongoDocument
  {
    public SmsModel(string content, string type, UserModel user, int providerId)
    {
      Content = content;
      Type = type;
      User = user;
      ProviderId = providerId;
    }

    public SmsModel()
    {

    }

    [BsonRequired]
    public string Content { get; set; }

    [BsonRequired]
    public string Type { get; set; }

    [BsonRequired]
    public int TypeId { get; set; }

    [BsonRequired]
    public UserModel User { get; set; }

    [BsonRequired]
    public int ProviderId { get; set; }

    [BsonRequired]
    public string ProviderName { get; set; }


  }



}
