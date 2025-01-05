using System.Linq.Expressions;
using Udemy.Common.Base;
using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Entities;

namespace Udemy.Course.Domain.Interfaces.Repository;

public interface IEnrollmentRepository : IBaseRepository<Enrollment>
{
    Task<IEnumerable<Enrollment>> GetAllAsync(Guid userId, Guid courseId, EndpointFilter filter);
    Task<Enrollment?> GetByIdAsync(Guid consumerId, Guid enrollmentId);
    Task<IEnumerable<Enrollment>> GetManyAsync(Expression<Func<Enrollment, bool>> predicate, Guid courseId, EndpointFilter filter);
    Task<Guid> AddAsync(Enrollment entity, Guid courseId);
    Task<IEnumerable<Enrollment>> GetAllByCourseIdAsync(Guid userId, Guid courseId, EndpointFilter filter);
    Task<IEnumerable<Enrollment>> GetAllByUserIdAsync(Guid userId, EndpointFilter filter);
    Task<IEnumerable<Enrollment>> GetUnpaidEnrollments(Guid courseId, EndpointFilter filter);
}