using SmsService.Dtos.User;
using SmsService.Entities;

namespace SmsService.Utils.Mappers
{
  public static class UserMapper
  {
    public static UserDto CreateUserDto(UserModel userModel)
      => new UserDto(userModel.Id, userModel.FirstName, userModel.LastName, userModel.PhoneNumber);
  }
}
