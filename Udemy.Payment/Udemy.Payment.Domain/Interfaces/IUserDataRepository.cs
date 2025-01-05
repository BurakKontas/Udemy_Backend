using Udemy.Common.Base;
using Udemy.Payment.Domain.Entities;

namespace Udemy.Payment.Domain.Interfaces;

public interface IUserDataRepository : IBaseRepository<UserDataEntity>
{
    Task<IEnumerable<UserDataEntity>> GetAllAsync();
    Task CreateAsync(UserDataEntity userData);
    Task DeleteAsync(Guid userId);
}