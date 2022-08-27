using Kavenegar.Models;
using Microsoft.Extensions.Options;
using SmsService.Dtos;
using SmsService.Dtos.Provider;
using SmsService.Dtos.Sms;
using SmsService.Entities;
using SmsService.Percistance;
using static SmsService.Percistance.BaseData;

namespace SmsService.Mappers;
public static class SmsMappers
{
  public static SmsModel CreateSmsModel(this SmsInputDto sendSmsInputDto)
  => new SmsModel(sendSmsInputDto.Content, sendSmsInputDto.TypeId, sendSmsInputDto.User, sendSmsInputDto.ProviderId);

  public static SmsModel CreateSmsModel(this SmsModel smsModel,SmsInputDto sendSmsInputDto)

  {
    smsModel.Content = sendSmsInputDto.Content;
    smsModel.TypeId = sendSmsInputDto.TypeId;
    smsModel.Receiever = sendSmsInputDto.User;
    smsModel.Provider = new Provider(sendSmsInputDto.ProviderId,null);
    return smsModel;
  }


  public static SmsReturnDto CreateSmsReturnDto(this SmsModel smsModel)
    => new SmsReturnDto(smsModel.Id.ToString(), smsModel.Content,
                            smsModel.Type, smsModel.Receiever.Id,
                            smsModel.Provider.Id);

  public static SmsModel CreateLoginSmsModel(this LoginSmsInputDto loginSmsInput)
  {
    var smsContent = string.Format(SmsMessagesTemplates.LoginSmsTemplate,
                                    loginSmsInput.appName,
                                    loginSmsInput.code);

    SmsModel smsModel = new(smsContent, SmsTypes.OPT.Id, loginSmsInput.User, loginSmsInput.ProviderId);

    return smsModel;
  }
  public static SmsModel UpdateSmsModel(SmsModel sms, UpdateSmsDto updateSmsDto)
  {
    sms.Cost = updateSmsDto.Cost;
    sms.IsSent = updateSmsDto.IsSent;
    sms.StatusMessage = updateSmsDto.StatusMessage;
    sms.Status = updateSmsDto.Status;
    sms.MessageId = updateSmsDto.messageId;
    return sms;
  }


  public static UpdateSmsDto CreateSentModel(SendResult sendResult)
    => new UpdateSmsDto(true, sendResult.Cost, sendResult.Status, sendResult.StatusText, sendResult.Messageid);

  public static UpdateSmsDto CreateNotSentModel(SendResult sendResult)
   => new UpdateSmsDto(false, sendResult.Cost, sendResult.Status, sendResult.StatusText, sendResult.Messageid);
}

