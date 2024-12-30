using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Udemy.Common.Base;
using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Entities;
using Udemy.Course.Domain.Interfaces.Repository;
using Udemy.Course.Infrastructure.Contexts;

namespace Udemy.Course.Infrastructure.Repositories;

public class CourseRepository(IElasticSearchRepository elasticSearchRepository) : ICourseRepository
{
    private readonly IElasticSearchRepository _elasticSearchRepository = elasticSearchRepository;

    public async Task<IEnumerable<Domain.Entities.Course>> GetCoursesByInstructorAsync(Guid instructorId, EndpointFilter filter)
    {
        var query = $"instructorIds:{instructorId}";
        return await _elasticSearchRepository.SearchAsync<Domain.Entities.Course>(query, filter);
    }

    public async Task<IEnumerable<Domain.Entities.Course>> GetFeaturedCoursesAsync(EndpointFilter filter)
    {
        var query = "isFeatured:true";
        var courses = await _elasticSearchRepository.SearchAsync<Domain.Entities.Course>(query, filter);
        return courses;
    }

    public async Task<IEnumerable<Domain.Entities.Course>> GetCoursesByKeywordAsync(string keyword, EndpointFilter filter)
    {
        var query = $"title:{keyword} OR description:{keyword}";
        var courses = await _elasticSearchRepository.SearchAsync<Domain.Entities.Course>(query, filter);
        return courses;
    }

    public IQueryable<Domain.Entities.Course> GetAll(EndpointFilter filter)
    {
        // ReSharper disable once SuspiciousTypeConversion.Global
        return (_elasticSearchRepository.SearchAsync<Domain.Entities.Course>("*", filter) as IQueryable<Domain.Entities.Course>)!;
    }

    public async Task<Domain.Entities.Course?> GetByIdAsync(Guid id)
    {
        return await _elasticSearchRepository.GetByIdAsync<Domain.Entities.Course>(id.ToString());
    }

    public async Task<IEnumerable<Domain.Entities.Course>> GetManyAsync(Expression<Func<Domain.Entities.Course, bool>> predicate, EndpointFilter filter)
    {
        return await _elasticSearchRepository.SearchAsync<Domain.Entities.Course>(predicate.ToString(), filter);
    }

    public async Task AddAsync(Domain.Entities.Course entity)
    {
        await _elasticSearchRepository.IndexAsync(entity);
    }

    public async Task AddManyAsync(IEnumerable<Domain.Entities.Course> entities)
    {
        await _elasticSearchRepository.BulkIndexAsync(entities);
    }

    public async Task UpdateAsync(Domain.Entities.Course entity)
    {
        await _elasticSearchRepository.UpdateAsync(entity.Id.ToString(), entity);
    }

    public async Task UpdateAsync(Domain.Entities.Course entity, Dictionary<string, object> updatedValues)
    {
        await _elasticSearchRepository.UpdateAsync(entity.Id.ToString(), updatedValues);
    }

    public async Task UpdateManyAsync(IEnumerable<Domain.Entities.Course> entities)
    {
        await _elasticSearchRepository.BulkIndexAsync(entities);
    }

    public async Task DeleteAsync(Domain.Entities.Course entity)
    {
        await _elasticSearchRepository.DeleteAsync<Domain.Entities.Course>(entity.Id.ToString());
    }

    public async Task DeleteManyAsync(IEnumerable<Domain.Entities.Course> entities)
    {
        var ids = entities.Select(e => e.Id.ToString());
        await _elasticSearchRepository.BulkDeleteAsync<Domain.Entities.Course>(ids);
    }

    public async Task<bool> ExistsAsync(Expression<Func<Domain.Entities.Course, bool>> predicate)
    {
        var query = predicate.ToString();
        var result = await _elasticSearchRepository.CountAsync<Domain.Entities.Course>(query);
        return result > 0;
    }
}