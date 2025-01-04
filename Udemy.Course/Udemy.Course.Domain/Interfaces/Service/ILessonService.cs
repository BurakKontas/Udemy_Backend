using System.Linq.Expressions;
using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Entities;

namespace Udemy.Course.Domain.Interfaces.Service;

public interface ILessonService
{
    Task<Guid> AddAsync(Guid userId, Guid categoryId, string title, string videoUrl, TimeSpan duration, string? description);
    Task<Lesson?> GetByIdAsync(Guid id, Guid categoryId);
    Task<IEnumerable<Lesson>> GetManyAsync(Expression<Func<Lesson, bool>> predicate, Guid categoryId, EndpointFilter filter);
    Task<Guid> UpdateAsync(Guid userId, Guid id, Dictionary<string, object> updates);
    Task<Guid> DeleteAsync(Guid userId, Guid id);
    Task<IEnumerable<Lesson>> GetAllAsync(EndpointFilter filter);
    Task<Lesson?> GetByIdAsync(Guid id);
    Task<IEnumerable<Lesson>> GetManyAsync(Expression<Func<Lesson, bool>> predicate, EndpointFilter filter);
    Task<Guid> AddAsync(Lesson entity, Guid categoryId);
    Task<IEnumerable<Lesson>> GetAllAsync(Guid categoryId, EndpointFilter filter);
    Task<IEnumerable<Lesson>> GetByCourseIdAsync(Guid courseId, EndpointFilter filter);
    Task<IEnumerable<Lesson>> GetByCategoryIdAsync(Guid categoryId, EndpointFilter filter);
}