using SmsService.Dtos;
using SmsService.Dtos.Provider;
using SmsService.Dtos.Sms;
using SmsService.Entities;
using SmsService.Percistance;

namespace SmsService.Mappers;
public static class SmsMappers
{
  public static SmsModel CreateSmsModel(this SmsInputDto sendSmsInputDto)
  => new SmsModel(sendSmsInputDto.Content, BaseDataMappers.GetSmsTypeById(sendSmsInputDto.TypeId),
                  sendSmsInputDto.User,sendSmsInputDto.ProviderId);

  public static SmsReturnDto CreateSmsReturnDto(this SmsModel smsModel)
    => new SmsReturnDto(smsModel.Id.ToString(), smsModel.Content,
                            smsModel.Type, smsModel.User.Id,
                            smsModel.ProviderId);


}

