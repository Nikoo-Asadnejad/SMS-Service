using MongoDB.Bson;
using SmsService.Dtos.User;
namespace SmsService.Dtos.Sms;
public record SmsInputDto(int TypeId, string Content, int ProvideId, UserDto User);

