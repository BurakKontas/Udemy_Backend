using Udemy.Common.Base;
using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Entities;

namespace Udemy.Course.Domain.Interfaces.Repository;

public interface ILikeRepository : IBaseRepository<Like>
{
    Task<IEnumerable<Like>> GetLikesByQuestionIdAsync(Guid questionId, EndpointFilter filter);
    Task<IEnumerable<Like>> GetLikesByAnswerIdAsync(Guid answerId, EndpointFilter filter);
    Task<IEnumerable<Like>> GetLikesByUserIdAsync(Guid userId, EndpointFilter filter);
    Task<Guid> AddAsync(Guid answerId, Like like);
    Task DeleteAsync(Guid answerId, Guid likeId);
    Task<bool> IsLiked(Guid userId, Guid answerId);
    Task<Like> GetLikeByIdAsync(Guid userId, Guid likeId);
    Task<Like> GetLikeByUserIdAsync(Guid userId, Guid answerId);
}