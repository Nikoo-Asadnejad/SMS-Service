using ErrorHandlingDll.ReturnTypes;
using MongoDB.Driver;
using MongoRepository.Repository;
using SmsService.Dtos.Sms;
using SmsService.Entities;
using SmsService.Interfaces;
using SmsService.Mappers;

namespace SmsService.Services
{
  public class SmsService : ISmsService
  {
    private readonly IMongoRepository<SmsModel> _smsRepository;
    public SmsService(IMongoRepository<SmsModel> smsRepository)
    {
      _smsRepository = smsRepository;
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

    public async Task<bool> UpdateSms(SmsModel sms ,UpdateSmsDto updateSmsDto)
    {
      SmsModel updatedSmsModel = SmsMappers.UpdateSmsModel(sms,updateSmsDto);     
      await _smsRepository.ReplaceOneAsync(updatedSmsModel);

      return true;
    }


  }
}
