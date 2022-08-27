using SmsService.Dtos.User;
using System.ComponentModel.DataAnnotations;

namespace SmsService.Dtos.Sms;
public record LoginSmsInputDto([Required] string code,[Required] string appName, [Required] int ProviderId,
  [Required] UserDto User);


