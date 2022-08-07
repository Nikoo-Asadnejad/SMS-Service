using ErrorHandlingDll.ReturnTypes;
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
    public async Task<ReturnModel<string>> CreateSmsAsync(SmsInputDto sendSmsInputDto )
    {
      ReturnModel<string> result = new();

      SmsModel smsModel = sendSmsInputDto.CreateSmsModel();
      await _smsRepository.InsertAsync(smsModel);
     
      result.CreateSuccessModel(data: smsModel.Id.ToString());
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
  }
}
