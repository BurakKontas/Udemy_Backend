using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Entities;

namespace Udemy.Course.Domain.Interfaces.Service;

public interface IAnswerService
{
    Task<IEnumerable<Answer>> GetAllAsync(EndpointFilter filter);
    Task<Answer> GetByIdAsync(Guid id);
    Task<Guid> AddAsync(Guid userId, Guid questionId, string value);
    Task<Guid> UpdateAsync(Guid userId, Guid answerId, Dictionary<string, object> updates);
    Task<Guid> DeleteAsync(Guid userId, Guid id);
    Task<IEnumerable<Answer>> GetAnswersByQuestionIdAsync(Guid questionId, EndpointFilter filter);
    Task<IEnumerable<Answer>> GetAnswersByUserIdAsync(Guid userId, EndpointFilter filter);
    Task<IEnumerable<Answer>> GetAnswersByQuestionIdAndUserIdAsync(Guid questionId, Guid userId, EndpointFilter filter);
    Task<Guid> AddAsync(Guid questionId, Answer answer);
}