using SmsService.Dtos.User;

namespace SmsService.Entities
{
  public class UserModel
  {
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
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }

    internal UserDto CreateUserDto()
    {
      throw new NotImplementedException();
    }
  }
}
