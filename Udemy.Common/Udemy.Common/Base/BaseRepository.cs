using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json.Linq;
using Udemy.Common.Helpers;
using Udemy.Common.ModelBinder;
using Consul.Filtering;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

    public virtual async Task<IEnumerable<T>> GetAll(EndpointFilter filter)
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


    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    protected virtual IQueryable<T> Filtering(IQueryable<T> query, EndpointFilter filter)
    {
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

        return query;
    }

    protected virtual IQueryable<T> Sorting(IQueryable<T> query, EndpointFilter filter)
    {
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

        return query;
    }

    protected virtual IQueryable<T> Paginating(IQueryable<T> query, EndpointFilter filter)
    {
        // Paginating
        query = query.Skip(filter.Start).Take(filter.Limit);

        return query;
    }

    public virtual async Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> predicate, EndpointFilter filter)
    {
        var query = _dbSet.AsNoTracking().Where(predicate);

        // Filtering
        query = Filtering(query, filter);

        // Sorting
        query = Sorting(query, filter);

        // Paginating
        query = Paginating(query, filter);

        return await query.ToListAsync();
    }

    public virtual async Task<Guid> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();

        return entity.Id;
    }

    public virtual async Task<Guid[]> AddManyAsync(IEnumerable<T> entities)
    {
        var entityArray = entities as T[] ?? entities.ToArray();
        if (!entityArray.Any()) return [];

        await _dbSet.AddRangeAsync(entityArray);
        await _context.SaveChangesAsync();

        return entityArray.Select(x => x.Id).ToArray();
    }

    public virtual async Task<T> UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    public virtual async Task<T> UpdateAsync(T entity, Dictionary<string, object> updatedValues)
    {
        try
        {
            if (updatedValues == null || !updatedValues.Any())
                throw new ArgumentException("No values provided to update.");

            var entry = _context.Entry(entity);

            foreach (var kvp in updatedValues)
            {
                if (kvp.Key.Contains('.'))
                {
                    var objects = kvp.Key.Split('.');
                    var key = string.Join(" ", objects.Take(objects.Length - 1));
                    var obj = entry.Navigation(key);
                    var property = obj.CurrentValue?.GetType().GetProperty(objects.Last());

                    if (property == null) continue;

                    if (kvp.Value is JsonElement jsonValue)
                    {
                        switch (jsonValue.ValueKind)
                        {
                            case JsonValueKind.String:
                                property.SetValue(obj.CurrentValue, jsonValue.GetString());
                                break;
                            case JsonValueKind.Number:
                                property.SetValue(obj.CurrentValue, jsonValue.GetDecimal());
                                break;
                            case JsonValueKind.True or JsonValueKind.False:
                                property.SetValue(obj.CurrentValue, jsonValue.GetBoolean());
                                break;
                        }
                    }
                    else
                    {
                        property.SetValue(obj.CurrentValue, kvp.Value);
                    }
                }
                else
                {
                    var property = entry.Property(kvp.Key);
                    if (kvp.Value is JsonElement jsonValue)
                    {
                        switch (jsonValue.ValueKind)
                        {
                            case JsonValueKind.String:
                                property.CurrentValue = jsonValue.GetString();
                                break;
                            case JsonValueKind.Number:
                                property.CurrentValue = jsonValue.GetDecimal();
                                break;
                            case JsonValueKind.True or JsonValueKind.False:
                                property.CurrentValue = jsonValue.GetBoolean();
                                break;
                        }
                    }
                    else
                    {
                        property.CurrentValue = kvp.Value;
                    }
                }
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

    public virtual async Task<Guid[]> UpdateManyAsync(IEnumerable<T> entities)
    {
        var entityArray = entities as T[] ?? entities.ToArray();
        if (!entityArray.Any()) return [];

        _dbSet.UpdateRange(entityArray);
        await _context.SaveChangesAsync();

        return entityArray.Select(x => x.Id).ToArray();
    }

    public virtual async Task<Guid> DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();

        return entity.Id;
    }

    public virtual async Task<Guid[]> DeleteManyAsync(IEnumerable<T> entities)
    {
        var entityArray = entities as T[] ?? entities.ToArray();
        if (!entityArray.Any()) return [];

        _dbSet.RemoveRange(entityArray);
        await _context.SaveChangesAsync();

        return entityArray.Select(x => x.Id).ToArray();
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }
}
