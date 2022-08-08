using Microsoft.Extensions.Options;
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
                  sendSmsInputDto.TypeId,
                  sendSmsInputDto.User,sendSmsInputDto.ProviderId ,
                  BaseDataMappers.GetProviderById(sendSmsInputDto.ProviderId),
                  BaseDataMappers.GetSenderPhoneByProviderId(sendSmsInputDto.ProviderId));

  public static SmsReturnDto CreateSmsReturnDto(this SmsModel smsModel)
    => new SmsReturnDto(smsModel.Id.ToString(), smsModel.Content,
                            smsModel.Type, smsModel.Receiver.Id,
                            smsModel.ProviderId);

  public static SmsModel UpdateSmsModel(SmsModel sms , UpdateSmsDto updateSmsDto)
  {
    sms.Cost = updateSmsDto.Cost;
    sms.IsSuccessfull = updateSmsDto.isSuccessful;
    sms.ProviderResult = updateSmsDto.ProviderResult;
    sms.SendingStatus = updateSmsDto.SendingStatus;

    return sms;
  }

  public static UpdateSmsDto CreateSuccessedUpdateModel(int cost, int sendingStatus,
                                                string statusMessage, long messageId,
                                                string message , long date , DateTime georgianDate)
    => new UpdateSmsDto(true, cost, new SendingStatusModel(sendingStatus, statusMessage),
                        new ProviderResultModel(messageId, message ,date, georgianDate));

  public static UpdateSmsDto CreateFailedUpdateModel(string statusMessage)
   => new UpdateSmsDto(false, null, new SendingStatusModel(0, statusMessage),null);
}

