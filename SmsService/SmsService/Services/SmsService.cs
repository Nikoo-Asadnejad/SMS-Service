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
    public async Task<ReturnModel<SmsModel>> CreateSmsAsync(SmsInputDto sendSmsInputDto )
    {
      ReturnModel<SmsModel> result = new();

      SmsModel smsModel = sendSmsInputDto.CreateSmsModel();
      await _smsRepository.InsertAsync(smsModel);
     
      result.CreateSuccessModel(data: smsModel);
      return result;
    }

    public async Task<ReturnModel<SmsReturnDto>> GetSmsAsync(string id)
    {
      ReturnModel<SmsReturnDto> result = new();

      var smsModel =await _smsRepository.FindByIdAsync(id);
      SmsReturnDto getSmsResultDto = smsModel.CreateSmsReturnDto();

      result.CreateSuccessModel(data: getSmsResultDto);
      return result;
    }

    public async Task<bool> UpdateSms(SmsModel sms ,UpdateSmsDto updateSmsDto)
    {
      SmsModel updatedSmsModel = SmsMappers.UpdateSmsModel(sms,updateSmsDto);
      UpdateDefinition<SmsModel> updateDefinition =  Builders<SmsModel>.Update.Set(s =>
      s.IsSuccessfull, updatedSmsModel.IsSuccessfull);
      
      await _smsRepository.ReplaceOneAsync(updatedSmsModel);

      return true;
    }


  }
}
