using MongoDB.Bson;
using SmsService.Dtos.User;
using System.ComponentModel.DataAnnotations;

namespace SmsService.Dtos.Sms;
public record SmsInputDto([Required] int TypeId, [Required] string Content, [Required] int ProviderId,[Required] UserDto User);

