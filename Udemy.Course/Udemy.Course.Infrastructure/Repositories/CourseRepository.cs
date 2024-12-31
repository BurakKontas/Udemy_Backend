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
        var query = $"title:{keyword}~ OR description:{keyword}~"; // ~ makes query fuzzy
        var courses = await _elasticSearchRepository.SearchAsync<Domain.Entities.Course>(query, filter);
        return courses;
    }

    public async Task<IEnumerable<Domain.Entities.Course>> GetAll(EndpointFilter filter)
    {
        // ReSharper disable once SuspiciousTypeConversion.Global
        return await _elasticSearchRepository.SearchAsync<Domain.Entities.Course>("*", filter);
    }

    public async Task<Domain.Entities.Course?> GetByIdAsync(Guid id)
    {
        var result = await _elasticSearchRepository.GetByIdAsync<Domain.Entities.Course>(id.ToString());
        return result;
    }

    public async Task<IEnumerable<Domain.Entities.Course>> GetManyAsync(Expression<Func<Domain.Entities.Course, bool>> predicate, EndpointFilter filter)
    {
        return await _elasticSearchRepository.SearchAsync<Domain.Entities.Course>(predicate.ToString(), filter);
    }

    public async Task<Guid> AddAsync(Domain.Entities.Course entity)
    {
        await _elasticSearchRepository.IndexAsync(entity);
        return entity.Id;
    }

    public async Task<Guid[]> AddManyAsync(IEnumerable<Domain.Entities.Course> entities)
    {
        var courses = entities as Domain.Entities.Course[] ?? entities.ToArray();
        if (!courses.Any()) return [];

        await _elasticSearchRepository.BulkIndexAsync(courses);
        return courses.Select(x => x.Id).ToArray();
    }

    public async Task<Guid> UpdateAsync(Domain.Entities.Course entity)
    {
        await _elasticSearchRepository.UpdateAsync(entity.Id.ToString(), entity);
        return entity.Id;
    }

    public async Task<Guid> UpdateAsync(Domain.Entities.Course entity, Dictionary<string, object> updatedValues)
    {
        await _elasticSearchRepository.UpdateAsync(entity.Id.ToString(), updatedValues);
        return entity.Id;
    }

    public async Task<Guid[]> UpdateManyAsync(IEnumerable<Domain.Entities.Course> entities)
    {
        var courses = entities as Domain.Entities.Course[] ?? entities.ToArray();
        if (!courses.Any()) return [];

        await _elasticSearchRepository.BulkIndexAsync(courses);
        return courses.Select(x => x.Id).ToArray();
    }

    public async Task<Guid> DeleteAsync(Domain.Entities.Course entity)
    {
        await _elasticSearchRepository.DeleteAsync<Domain.Entities.Course>(entity.Id.ToString());
        return entity.Id;
    }

    public async Task<Guid[]> DeleteManyAsync(IEnumerable<Domain.Entities.Course> entities)
    {
        var courses = entities as Domain.Entities.Course[] ?? entities.ToArray();
        if (!courses.Any()) return [];

        var ids = courses.Select(e => e.Id.ToString());
        await _elasticSearchRepository.BulkDeleteAsync<Domain.Entities.Course>(ids);
        return courses.Select(x => x.Id).ToArray();
    }

    public async Task<bool> ExistsAsync(Expression<Func<Domain.Entities.Course, bool>> predicate)
    {
        var query = predicate.ToString();
        var result = await _elasticSearchRepository.CountAsync<Domain.Entities.Course>(query);
        return result > 0;
    }
}