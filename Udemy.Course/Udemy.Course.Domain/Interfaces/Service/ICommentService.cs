using System.Security.Cryptography.X509Certificates;
using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Entities;

namespace Udemy.Course.Domain.Interfaces.Service;

public interface ICommentService
{
    Task<IEnumerable<Comment>> GetAll(Guid courseId, EndpointFilter filter);
    Task<Comment?> GetByIdAsync(Guid id, Guid courseId);
    Task<IEnumerable<Comment>> GetCommentsByCourseIdAsync(Guid courseId, EndpointFilter filter);
    Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(Guid userId, EndpointFilter filter);
    Task<Guid> AddAsync(Guid userId, Guid courseId, string value, int rateValue);
    Task<Guid> UpdateAsync(Guid commentId, Dictionary<string, object> updates);
    Task<Guid> DeleteAsync(Guid id);
    Task<Comment> GetByIdAsync(Guid id);
}