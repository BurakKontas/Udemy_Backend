using System.Linq.Expressions;
using Udemy.Common.ModelBinder;

namespace Udemy.Common.Base;

public interface IBaseRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAll(EndpointFilter filter);
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> predicate, EndpointFilter filter);
    Task<Guid> AddAsync(T entity);
    Task<Guid[]> AddManyAsync(IEnumerable<T> entities);
    Task<Guid> UpdateAsync(T entity);
    Task<Guid> UpdateAsync(T entity, Dictionary<string, object> updatedValues);
    Task<Guid[]> UpdateManyAsync(IEnumerable<T> entities);
    Task<Guid> DeleteAsync(T entity);
    Task<Guid[]> DeleteManyAsync(IEnumerable<T> entities);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
}