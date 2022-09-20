using MongoRepository.Repository;
using SmsService.Entities;

namespace SmsService.DataAccess.Repository
{
  public class UnitOfWork : IUnitOfWork
  {
    public IMongoRepository<SmsModel> SmsRepository { get; private set; }
    public UnitOfWork(IMongoRepository<SmsModel> smsRepository)
    {
      SmsRepository = smsRepository;
    }
  }
}
