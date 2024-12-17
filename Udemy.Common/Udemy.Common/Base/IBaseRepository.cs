using System.Linq.Expressions;

namespace Udemy.Common.Base;

public interface IBaseRepository<T> where T : class
{
    IQueryable<T> GetAll();
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    Task AddManyAsync(IEnumerable<T> entities);
    Task UpdateAsync(T entity);
    Task UpdateManyAsync(IEnumerable<T> entities);
    Task DeleteAsync(T entity);
    Task DeleteManyAsync(IEnumerable<T> entities);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
}