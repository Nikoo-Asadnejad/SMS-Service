using MongoRepository.Repository;
using SmsService.Entities;

namespace SmsService.DataAccess.Repository
{
  public interface IUnitOfWork
  {
    IMongoRepository<SmsModel> SmsRepository { get; }
  }
}
