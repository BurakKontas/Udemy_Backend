﻿using System.Linq.Expressions;
using Udemy.Common.ModelBinder;

namespace Udemy.Common.Base;

public interface IBaseRepository<T> where T : class
{
    IQueryable<T> GetAll(EndpointFilter filter);
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> predicate, EndpointFilter filter);
    Task AddAsync(T entity);
    Task AddManyAsync(IEnumerable<T> entities);
    Task UpdateAsync(T entity);
    Task UpdateAsync(T entity, Dictionary<string, object> updatedValues);
    Task UpdateManyAsync(IEnumerable<T> entities);
    Task DeleteAsync(T entity);
    Task DeleteManyAsync(IEnumerable<T> entities);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
}