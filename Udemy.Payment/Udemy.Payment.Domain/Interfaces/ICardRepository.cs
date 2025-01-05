using Udemy.Common.Base;
using Udemy.Payment.Domain.Entities;

namespace Udemy.Payment.Domain.Interfaces;

public interface ICardRepository : IBaseRepository<CardEntity>
{
    Task<CardEntity?> GetByIdAsync(string cardId);
    Task<IEnumerable<CardEntity>> GetAllAsync();
    Task CreateAsync(CardEntity card);
    Task DeleteAsync(string cardId);
}