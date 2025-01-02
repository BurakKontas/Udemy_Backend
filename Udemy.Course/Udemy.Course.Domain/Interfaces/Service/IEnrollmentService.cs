using System.Linq.Expressions;
using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Entities;

namespace Udemy.Course.Domain.Interfaces.Service;

public interface IEnrollmentService
{
    Task<Guid> AddAsync(Enrollment entity);
    Task<IEnumerable<Enrollment>> GetAll(Guid courseId, EndpointFilter filter);
    Task<Enrollment?> GetByIdAsync(Guid id, Guid courseId);
    Task<IEnumerable<Enrollment>> GetManyAsync(Expression<Func<Enrollment, bool>> predicate, Guid courseId, EndpointFilter filter);
    Task<Guid> UpdateAsync(Guid id, Dictionary<string, object> updates);
    Task<Guid> DeleteAsync(Guid id);
    Task<IEnumerable<Enrollment>> GetAllAsync(EndpointFilter filter);
    Task<Enrollment?> GetByIdAsync(Guid id);
    Task<IEnumerable<Enrollment>> GetManyAsync(Expression<Func<Enrollment, bool>> predicate, EndpointFilter filter);
    Task<Guid> AddAsync(Enrollment entity, Guid courseId);
    Task<IEnumerable<Enrollment>> GetAllAsync(Guid courseId, EndpointFilter filter);
}