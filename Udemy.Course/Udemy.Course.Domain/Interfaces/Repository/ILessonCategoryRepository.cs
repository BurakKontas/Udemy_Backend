using System.Linq.Expressions;
using Udemy.Common.Base;
using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Entities;

namespace Udemy.Course.Domain.Interfaces.Repository;

public interface ILessonCategoryRepository : IBaseRepository<LessonCategory>
{
    Task<IEnumerable<LessonCategory>> GetAll(Guid courseId, EndpointFilter filter);
    Task<LessonCategory?> GetByIdAsync(Guid id, Guid courseId);
    Task<IEnumerable<LessonCategory>> GetManyAsync(Expression<Func<LessonCategory, bool>> predicate, Guid courseId, EndpointFilter filter);
    Task<Guid> AddAsync(LessonCategory entity, Guid courseId);  
    Task<IEnumerable<LessonCategory>> GetByCourseIdAsync(Guid courseId, EndpointFilter filter);
}