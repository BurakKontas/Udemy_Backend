using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Entities;

namespace Udemy.Course.Domain.Interfaces.Service;

public interface ILikeService
{
    Task<IEnumerable<Like>> GetLikesByAnswerIdAsync(Guid answerId, EndpointFilter filter);
    Task<IEnumerable<Like>> GetLikesByUserIdAsync(Guid userId, EndpointFilter filter);
    Task<Guid> AddAsync(Guid answerId, Like like);
    Task DeleteAsync(Guid answerId, Guid likeId);
    Task<bool> IsLiked(Guid userId, Guid answerId);
    Task<Like> GetLikeByIdAsync(Guid userId, Guid likeId);
    Task<IEnumerable<Like>> GetAllAsync(EndpointFilter filter);
    Task<Like> GetByIdAsync(Guid id);
    Task<Guid> UpdateAsync(Like like, Dictionary<string, object> updates);
    Task<Guid> DeleteAsync(Guid id);
}