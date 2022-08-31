using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoRepository.Atrributes;
using MongoRepository.Models;
using SmsService.Mappers;

namespace SmsService.Entities
{

  [MongoCollectionAttribute("Sms")]
  public class SmsModel : MongoDocument
  {

    [BsonRequired]
    public string Content { get; set; }

    [BsonRequired]
    public string Type { get; set; }

    [BsonRequired]
    public int TypeId { get; set; }

    [BsonRequired]
    public UserModel Receiever { get; set; }
    public Provider Provider { get; set; }

    public bool IsSent { get; set; }
    public long SentDate { get; set; }

    public bool IsDelivered { get; set; }

    public int? Cost { get; set; }

    public long? MessageId { get; set; }
    public int? Status { get; set; }
    public string StatusMessage { get; set; }

    public SmsModel()
    {

    }
    public SmsModel(string content, int typeId, UserModel reciever, int providerId)
    {
      Content = content;
      TypeId = typeId;
      Type = BaseDataMappers.GetSmsTypeById(typeId);
      Provider = new Provider(providerId, BaseDataMappers.GetProviderById(providerId),
                              BaseDataMappers.GetSenderPhoneByProviderId(providerId));
      Receiever = reciever;
    }
  }



}
