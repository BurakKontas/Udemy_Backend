using System.Linq.Expressions;
using Udemy.Common.Base;
using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Entities;

namespace Udemy.Course.Domain.Interfaces.Repository;

public interface IEnrollmentRepository : IBaseRepository<Enrollment>
{
    Task<IEnumerable<Enrollment>> GetAll(Guid courseId, EndpointFilter filter);
    Task<Enrollment?> GetByIdAsync(Guid id, Guid courseId);
    Task<IEnumerable<Enrollment>> GetManyAsync(Expression<Func<Enrollment, bool>> predicate, Guid courseId, EndpointFilter filter);
    Task<Guid> AddAsync(Enrollment entity, Guid courseId);
    Task<IEnumerable<Enrollment>> GetAllByCourseId(Guid courseId, EndpointFilter filter);
}