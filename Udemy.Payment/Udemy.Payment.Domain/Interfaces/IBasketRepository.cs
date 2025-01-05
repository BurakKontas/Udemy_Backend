using Udemy.Common.Base;
using Udemy.Payment.Domain.Entities;

namespace Udemy.Payment.Domain.Interfaces;

public interface IBasketRepository : IBaseRepository<BasketEntity>
{
    Task<BasketEntity?> GetByIdAsync(string basketId);
    Task<IEnumerable<BasketEntity>> GetAllAsync();
    Task CreateAsync(BasketEntity basket);
    Task DeleteAsync(string basketId);
}