using Udemy.Common.Base;
using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Entities;

namespace Udemy.Course.Domain.Interfaces.Repository;

public interface IFavoriteRepository : IBaseRepository<Favorite>
{
    Task<IEnumerable<Favorite>> GetFavoritesByUserIdAsync(Guid userId, EndpointFilter filter);
    Task<Guid> AddAsync(Guid userId, Favorite favorite);
    Task<Guid> DeleteAsync(Guid userId, Guid favoriteId);
    Task<bool> IsFavorite(Guid userId, Guid courseId);
    Task<Favorite> GetFavoriteByIdAsync(Guid userId, Guid favoriteId);
    Task<Favorite> GetFavoriteByUserIdAsync(Guid userId, Guid courseId);
    Task<IEnumerable<Favorite>> GetFavoritesByCourseIdAsync(Guid courseId, EndpointFilter filter);
}