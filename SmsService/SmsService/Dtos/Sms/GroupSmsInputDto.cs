using MongoDB.Bson;
using SmsService.Dtos.User;
using System.ComponentModel.DataAnnotations;

namespace SmsService.Dtos.Sms;
public record GroupSmsInputDto([Required] int TypeId, [Required] string Content, [Required] int ProviderId,
  [Required][MaxLength(200, ErrorMessage = "تعداد کاربران نباید بیشتر از 200 باشد")] List<UserDto> Users);

