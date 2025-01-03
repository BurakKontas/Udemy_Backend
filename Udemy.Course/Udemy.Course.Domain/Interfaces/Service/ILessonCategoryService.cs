using System.Linq.Expressions;
using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Entities;

namespace Udemy.Course.Domain.Interfaces.Service;

public interface ILessonCategoryService
{
    Task<Guid> AddAsync(Guid courseId, string name, string? description);
    Task<IEnumerable<LessonCategory>> GetAll(Guid courseId, EndpointFilter filter);
    Task<LessonCategory?> GetByIdAsync(Guid id, Guid courseId);
    Task<IEnumerable<LessonCategory>> GetManyAsync(Expression<Func<LessonCategory, bool>> predicate, Guid courseId, EndpointFilter filter);
    Task<Guid> UpdateAsync(Guid id, Dictionary<string, object> updates);
    Task<Guid> DeleteAsync(Guid id);
    Task<IEnumerable<LessonCategory>> GetAllAsync(EndpointFilter filter);
    Task<LessonCategory?> GetByIdAsync(Guid id);
    Task<IEnumerable<LessonCategory>> GetManyAsync(Expression<Func<LessonCategory, bool>> predicate, EndpointFilter filter);
    Task<Guid> AddAsync(LessonCategory entity, Guid courseId);
    Task<IEnumerable<LessonCategory>> GetAllAsync(Guid courseId, EndpointFilter filter);
    Task<IEnumerable<LessonCategory>> GetByCourseIdAsync(Guid courseId, EndpointFilter filter);
}