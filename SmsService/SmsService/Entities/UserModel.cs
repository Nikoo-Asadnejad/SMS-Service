using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoRepository.Atrributes;
using MongoRepository.Models;
using SmsService.Dtos.User;

namespace SmsService.Entities
{
  public class UserModel 
  {

    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }

    public UserModel(long id, string firstName, string lastName, string phoneNumber)
    {
      Id = id;
      FirstName = firstName;
      LastName = lastName;
      PhoneNumber = phoneNumber;
    }

    public UserModel()
    {

    }


  }
}
