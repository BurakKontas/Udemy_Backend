using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Udemy.Common.ModelBinder;

namespace Udemy.Common.Base;

public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    private readonly DbContext _context;
    private readonly DbSet<T> _dbSet;

    protected BaseRepository(DbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAll(EndpointFilter filter)
{
    var query = _dbSet.AsNoTracking();

    // Filtering
    if (!string.IsNullOrEmpty(filter.FilterBy) && !string.IsNullOrEmpty(filter.FilterValue))
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, filter.FilterBy);
        var value = Expression.Constant(filter.FilterValue);
        var condition = Expression.Equal(property, value);
        var predicate = Expression.Lambda<Func<T, bool>>(condition, parameter);

        query = query.Where(predicate);
    }

    // Sorting
    if (!string.IsNullOrEmpty(filter.SortBy))
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, filter.SortBy);
        var lambda = Expression.Lambda(property, parameter);

        var methodName = filter.SortOrder?.ToLower() == "desc" ? "OrderByDescending" : "OrderBy";
        var method = typeof(Queryable).GetMethods()
            .First(m => m.Name == methodName && m.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(T), property.Type);

        query = (IQueryable<T>)method.Invoke(null, [query, lambda])!;
    }

    // Paginating
    query = query.Skip(filter.Start).Take(filter.Limit);

    return await query.ToArrayAsync();
}


    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> predicate, EndpointFilter filter)
    {
        var query = _dbSet.AsNoTracking().Where(predicate);

        // Filtering
        if (!string.IsNullOrEmpty(filter.FilterBy) && !string.IsNullOrEmpty(filter.FilterValue))
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, filter.FilterBy);
            var value = Expression.Constant(filter.FilterValue);
            var condition = Expression.Equal(property, value);
            var filterPredicate = Expression.Lambda<Func<T, bool>>(condition, parameter);

            query = query.Where(filterPredicate);
        }

        // Sorting
        if (!string.IsNullOrEmpty(filter.SortBy))
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, filter.SortBy);
            var lambda = Expression.Lambda(property, parameter);

            var methodName = filter.SortOrder?.ToLower() == "desc" ? "OrderByDescending" : "OrderBy";
            var method = typeof(Queryable).GetMethods()
                .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), property.Type);

            query = (IQueryable<T>)method.Invoke(null, [query, lambda])!;
        }

        // Paginating
        query = query.Skip(filter.Start).Take(filter.Limit);

        return await query.ToListAsync();
    }

    public async Task<Guid> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();

        return entity.Id;
    }

    public async Task<Guid[]> AddManyAsync(IEnumerable<T> entities)
    {
        var entityArray = entities as T[] ?? entities.ToArray();
        if (!entityArray.Any()) return [];

        await _dbSet.AddRangeAsync(entityArray);
        await _context.SaveChangesAsync();

        return entityArray.Select(x => x.Id).ToArray();
    }

    public async Task<T> UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    public async Task<T> UpdateAsync(T entity, Dictionary<string, object> updatedValues)
    {
        try
        {
            if (updatedValues == null || !updatedValues.Any())
                throw new ArgumentException("No values provided to update.");

            var entry = _context.Entry(entity);

            foreach (var kvp in updatedValues)
            {
                var property = entry.Property(kvp.Key);

                if (property == null)
                    throw new ArgumentException($"Property {kvp.Key} not found on entity.");

                property.CurrentValue = kvp.Value;
            }

            _dbSet.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error updating entity.", ex);
        }

    }

    public async Task<Guid[]> UpdateManyAsync(IEnumerable<T> entities)
    {
        var entityArray = entities as T[] ?? entities.ToArray();
        if (!entityArray.Any()) return [];

        _dbSet.UpdateRange(entityArray);
        await _context.SaveChangesAsync();

        return entityArray.Select(x => x.Id).ToArray();
    }

    public async Task<Guid> DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();

        return entity.Id;
    }

    public async Task<Guid[]> DeleteManyAsync(IEnumerable<T> entities)
    {
        var entityArray = entities as T[] ?? entities.ToArray();
        if (!entityArray.Any()) return [];

        _dbSet.RemoveRange(entityArray);
        await _context.SaveChangesAsync();

        return entityArray.Select(x => x.Id).ToArray();
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }
}
