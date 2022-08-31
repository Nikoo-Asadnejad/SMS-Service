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

  public static SmsModel CreateNewSmsModel(this SmsModel smsModel, SmsInputDto sendSmsInputDto)

  {
    smsModel.Content = sendSmsInputDto.Content;
    smsModel.TypeId = sendSmsInputDto.TypeId;
    smsModel.Type = BaseDataMappers.GetSmsTypeById(sendSmsInputDto.TypeId);
    smsModel.Receiever = sendSmsInputDto.User;
    smsModel.Provider = new Provider(sendSmsInputDto.ProviderId, null);
    return smsModel;
  }
  public static SmsModel CreateLoginSmsModel(this SmsModel smsModel, LoginSmsInputDto loginSmsInput)
  {
    var smsContent = string.Format(SmsMessagesTemplates.LoginSmsTemplate,
                                    loginSmsInput.appName,
                                    loginSmsInput.code);

    smsModel.Content = smsContent;
    smsModel.TypeId = SmsTypes.OPT.Id;
    smsModel.Type = SmsTypes.OPT.Name;
    smsModel.Receiever = loginSmsInput.User;
    smsModel.Provider = new Provider(loginSmsInput.ProviderId, null);
    return smsModel;
  }
  public static SmsModel UpdateSmsModel(this SmsModel sms, SmsModel newSms)
  {
    sms.Cost = newSms.Cost;
    sms.IsSent = newSms.IsSent;
    sms.StatusMessage = newSms.StatusMessage;
    sms.Status = newSms.Status;
    sms.MessageId = newSms.MessageId;
    sms.Provider.Number = newSms.Provider.Number;
    sms.IsDelivered = newSms.IsDelivered;
    sms.SentDate = newSms.SentDate;

    return sms;
  }
  

 public static SmsModel UpdateSmsDeliveryStatus(this SmsModel sms, StatusResult deliveryResult, bool isDelivered)
  {
    sms.Status = (int)deliveryResult.Status;
    sms.StatusMessage = deliveryResult.Statustext;
    sms.IsDelivered = isDelivered;
 
    return sms;
  }
  public static SmsModel UpdateSmsStatus(this SmsModel sms, SendResult sendResult, bool isSent)
  {
    sms.IsSent = isSent;
    sms.StatusMessage = sendResult.StatusText;
    sms.Status = sendResult.Status;
    sms.MessageId = sendResult.Messageid;
    sms.Provider.Number = sendResult.Sender;
    sms.SentDate = sendResult.Date;

    return sms;
  }
  public static SmsReturnDto CreateSmsReturnDto(this SmsModel smsModel)
    => new SmsReturnDto(smsModel.Id.ToString(), smsModel.Content,
                            smsModel.Type, smsModel.Receiever.Id,
                            smsModel.Provider.Id);

}

