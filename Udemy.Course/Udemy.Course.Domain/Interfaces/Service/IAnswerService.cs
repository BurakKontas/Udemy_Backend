using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Entities;

namespace Udemy.Course.Domain.Interfaces.Service;

public interface IAnswerService
{
    Task<IEnumerable<Answer>> GetAllAsync(EndpointFilter filter);
    Task<Answer> GetByIdAsync(Guid id);
    Task<Guid> AddAsync(Answer answer);
    Task<Guid> UpdateAsync(Answer answer, Dictionary<string, object> updates);
    Task<Guid> DeleteAsync(Guid id);
    Task<IEnumerable<Answer>> GetAnswersByQuestionIdAsync(Guid questionId, EndpointFilter filter);
    Task<IEnumerable<Answer>> GetAnswersByUserIdAsync(Guid userId, EndpointFilter filter);
    Task<IEnumerable<Answer>> GetAnswersByQuestionIdAndUserIdAsync(Guid questionId, Guid userId, EndpointFilter filter);
    Task<Guid> AddAsync(Guid questionId, Answer answer);
    Task DeleteAsync(Guid questionId, Guid answerId);
}