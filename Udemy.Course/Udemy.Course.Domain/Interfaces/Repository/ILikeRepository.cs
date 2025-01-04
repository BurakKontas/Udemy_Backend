using Udemy.Common.Base;
using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Entities;

namespace Udemy.Course.Domain.Interfaces.Repository;

public interface ILikeRepository : IBaseRepository<Like>
{
    Task<IEnumerable<Like>> GetLikesByQuestionIdAsync(Guid questionId, EndpointFilter filter);
    Task<IEnumerable<Like>> GetLikesByUserIdAsync(Guid userId, EndpointFilter filter);
    Task<Guid> AddAsync(Guid questionId, Like like);
    Task DeleteAsync(Guid questionId, Guid likeId);
    Task<bool> IsLiked(Guid userId, Guid questionId);
    Task<Like> GetLikeByIdAsync(Guid userId, Guid likeId);
    Task<Like> GetLikeByUserIdAsync(Guid userId, Guid questionId);
    Task<int> GetLikesCountByQuestionIdAsync(Guid questionId);
}