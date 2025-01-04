using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Entities;

namespace Udemy.Course.Domain.Interfaces.Service;

public interface IFavoriteService
{
    Task<IEnumerable<Favorite>> GetAllAsync(EndpointFilter filter);
    Task<Favorite?> GetByIdAsync(Guid userId, Guid id);
    Task<Guid> AddAsync(Favorite favorite);
    Task<Guid> UpdateAsync(Favorite favorite, Dictionary<string, object> updates);
    Task DeleteAsync(Guid userId, Guid id);
    Task<IEnumerable<Favorite>> GetFavoritesByUserIdAsync(Guid userId, EndpointFilter filter);
    Task<Guid> AddAsync(Guid userId, Guid courseId);
    Task<bool> IsFavorite(Guid userId, Guid courseId);
}