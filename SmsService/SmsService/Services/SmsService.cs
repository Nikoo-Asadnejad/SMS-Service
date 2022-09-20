using ErrorHandlingDll.ReturnTypes;
using MongoDB.Driver;
using MongoRepository.Repository;
using SmsService.DataAccess.Repository;
using SmsService.Dtos.Sms;
using SmsService.Entities;
using SmsService.Interfaces;
using SmsService.Mappers;

namespace SmsService.Services
{
  public class SmsService : ISmsService
  {
    private readonly IMongoRepository<SmsModel> _smsRepository;
    public SmsService(IUnitOfWork unitOfWork)
    {
      _smsRepository = unitOfWork.SmsRepository;
    }
    public async Task<ReturnModel<SmsModel>> CreateSmsAsync(SmsModel smsModel )
    {
      ReturnModel<SmsModel> result = new();
      await _smsRepository.InsertAsync(smsModel);

      result.CreateSuccessModel(data: smsModel, title : "SMS");
      return result;
    }

    public async Task<ReturnModel<SmsReturnDto>> GetSmsAsync(string id)
    {
      ReturnModel<SmsReturnDto> result = new();

      var smsModel =await _smsRepository.FindByIdAsync(id);
      SmsReturnDto getSmsResultDto = smsModel.CreateSmsReturnDto();

      result.CreateSuccessModel(data: getSmsResultDto , title: "SMS");
      return result;
    }

    public async Task<ReturnModel<SmsModel>> GetSmsByMessageIdAsync(long messageId)
    {
      ReturnModel<SmsModel> result = new();
      SmsModel sms = await _smsRepository.FindAsync(s => s.MessageId == messageId);
      if(sms is null)
      {
        result.CreateNotFoundModel();
        return result;
      }

      result.CreateSuccessModel(data: sms);
      return result;
    }
    public async Task<bool> UpdateSms(SmsModel newSms)
    {  
      await _smsRepository.ReplaceOneAsync(newSms);
      return true;
    }


  }
}
