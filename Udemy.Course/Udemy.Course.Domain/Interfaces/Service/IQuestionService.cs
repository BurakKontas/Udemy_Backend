using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Entities;

namespace Udemy.Course.Domain.Interfaces.Service;

public interface IQuestionService
{
    Task<IEnumerable<Question>> GetAllAsync(EndpointFilter filter);
    Task<Question> GetByIdAsync(Guid id);
    Task<Guid> AddAsync(Guid userId, Guid lessonId, string value);
    Task<Guid> UpdateAsync(Guid userId, Guid questionId, Dictionary<string, object> updates);
    Task<Guid> DeleteAsync(Guid userId, Guid id);
    Task<IEnumerable<Question>> GetQuestionsByLessonIdAsync(Guid lessonId, EndpointFilter filter);
    Task<IEnumerable<Question>> GetQuestionsByUserIdAsync(Guid userId, EndpointFilter filter);
}