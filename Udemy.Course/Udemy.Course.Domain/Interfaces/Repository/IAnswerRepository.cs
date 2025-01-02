using Udemy.Common.Base;
using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Entities;

namespace Udemy.Course.Domain.Interfaces.Repository;

public interface IAnswerRepository : IBaseRepository<Answer>
{
    Task<IEnumerable<Answer>> GetAnswersByQuestionIdAsync(Guid questionId, EndpointFilter filter);
    Task<IEnumerable<Answer>> GetAnswersByUserIdAsync(Guid userId, EndpointFilter filter);
    Task<IEnumerable<Answer>> GetAnswersByQuestionIdAndUserIdAsync(Guid questionId, Guid userId, EndpointFilter filter);
    Task<Guid> AddAsync(Guid questionId, Answer answer);
    Task DeleteAsync(Guid questionId, Guid answerId);
}