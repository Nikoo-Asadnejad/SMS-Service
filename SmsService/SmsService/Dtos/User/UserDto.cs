using MongoDB.Bson;
using SmsService.Entities;

namespace SmsService.Dtos.User;
public class UserDto : UserModel 
{
  public UserDto(long id, string firstName, string lastName, string phoneNumber) :
    base(id,firstName,lastName,phoneNumber) 
  {

  }
};

