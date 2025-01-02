using Udemy.Common.Base;
using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Entities;

namespace Udemy.Course.Domain.Interfaces.Repository;

public interface ICommentRepository : IBaseRepository<Comment>
{
    Task<IEnumerable<Comment>> GetCommentsByCourseIdAsync(Guid courseId, EndpointFilter filter);
    Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(Guid userId, EndpointFilter filter);
}