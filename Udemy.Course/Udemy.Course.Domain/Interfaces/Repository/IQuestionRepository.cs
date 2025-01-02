using Udemy.Common.Base;
using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Entities;

namespace Udemy.Course.Domain.Interfaces.Repository;

public interface IQuestionRepository : IBaseRepository<Question>
{
    Task<IEnumerable<Question>> GetQuestionsByLessonIdAsync(Guid lessonId, EndpointFilter filter);
    Task<IEnumerable<Question>> GetQuestionsByUserIdAsync(Guid userId, EndpointFilter filter);
}