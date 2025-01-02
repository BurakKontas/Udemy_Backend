using System.Linq.Expressions;
using Udemy.Common.Base;
using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Entities;

namespace Udemy.Course.Domain.Interfaces.Repository;

public interface ILessonRepository : IBaseRepository<Lesson>
{
    Task<IEnumerable<Lesson>> GetAll(Guid categoryId, EndpointFilter filter);
    Task<Lesson?> GetByIdAsync(Guid id, Guid categoryId);
    Task<IEnumerable<Lesson>> GetManyAsync(Expression<Func<Lesson, bool>> predicate, Guid categoryId, EndpointFilter filter);
    Task<Guid> AddAsync(Lesson entity, Guid categoryId);
}