using System.Linq.Expressions;
using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Entities;

namespace Udemy.Course.Domain.Interfaces.Service;

public interface IEnrollmentService
{
    Task<Guid> AddAsync(Guid userId, Guid courseId);
    Task<IEnumerable<Enrollment>> GetAll(Guid userId, Guid courseId, EndpointFilter filter);
    Task<IEnumerable<Enrollment>> GetManyAsync(Expression<Func<Enrollment, bool>> predicate, Guid courseId, EndpointFilter filter);
    Task<Guid> UpdateAsync(Guid id, Dictionary<string, object> updates);
    Task<Guid> DeleteAsync(Guid id);
    Task<IEnumerable<Enrollment>> GetAllAsync(EndpointFilter filter);
    Task<Enrollment?> GetByIdAsync(Guid consumerId, Guid enrollmentId);
    Task<IEnumerable<Enrollment>> GetManyAsync(Expression<Func<Enrollment, bool>> predicate, EndpointFilter filter);
    Task<Guid> AddAsync(Enrollment entity, Guid courseId);
    Task<IEnumerable<Enrollment>> GetAllByCourseAsync(Guid userId, Guid courseId, EndpointFilter filter);
    Task<IEnumerable<Enrollment>> GetAllByUserAsync(Guid consumerId, Guid userId, EndpointFilter filter);
}