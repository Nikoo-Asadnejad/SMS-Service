using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoRepository.Atrributes;
using MongoRepository.Models;

namespace SmsService.Entities
{

  [MongoCollectionAttribute("Sms")]
  public class SmsModel : MongoDocument
  {
    public SmsModel(string content, string type,int typeId, UserModel user, int providerId ,
      string providerName , string senderPhoneNumber)
    {
      Content = content;
      Type = type;
      TypeId = typeId;
      Receiver = user;
      ProviderId = providerId;
      ProviderName = providerName;
      SenderPhoneNumber = senderPhoneNumber;
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
    public UserModel Receiver { get; set; }

    [BsonRequired]
    public int ProviderId { get; set; }

    [BsonRequired]
    public string ProviderName { get; set; }

    public bool? IsSuccessfull { get; set; }

    public string SenderPhoneNumber { get; set; }

    public int? Cost { get; set; }

    public SendingStatusModel SendingStatus{get; set;}

    public ProviderResultModel ProviderResult { get; set; }


  }



}
