using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Udemy.Common.Base;
using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Adapters;
using Udemy.Course.Domain.Dtos;
using Udemy.Course.Domain.Entities;
using Udemy.Course.Domain.Interfaces.Repository;
using Udemy.Course.Infrastructure.Contexts;

namespace Udemy.Course.Infrastructure.Repositories;

public class CourseRepository(IElasticSearchRepository elasticSearchRepository, ApplicationDbContext dbContext)
    : BaseRepository<Domain.Entities.Course>(dbContext), ICourseRepository
{
    private readonly IElasticSearchRepository _elasticSearchRepository = elasticSearchRepository;
    private readonly ApplicationDbContext _dbContext = dbContext;

    // from pg
    public async Task<IEnumerable<Domain.Entities.Course>> GetCoursesByInstructorAsync(Guid instructorId, EndpointFilter filter)
    {
        var query = _dbContext.Courses
            .Include(c => c.CourseDetails)
            .AsQueryable();

        query = query.Where(c => c.InstructorIds.Contains(instructorId));

        query = query
            .Skip(filter.Start)
            .Take(filter.Limit);

        return await query.ToArrayAsync();
    }

    // from pg
    public async Task<IEnumerable<Domain.Entities.Course>> GetFeaturedCoursesAsync(EndpointFilter filter)
    {
        var query = _dbContext.Courses
            .Include(c => c.CourseDetails)
            .AsQueryable();

        query = query.Where(c => c.IsFeatured);

        query = query
            .Skip(filter.Start)
            .Take(filter.Limit);

        return await query.ToArrayAsync();
    }

    // from elastic then get pg details
    public async Task<IEnumerable<Domain.Entities.Course>> GetCoursesByKeywordAsync(string keyword, EndpointFilter filter)
    {
        var elasticQuery = $"title:{keyword}~ OR description:{keyword}~ OR tags:{keyword}~"; // ~ makes query fuzzy
        var courseDtos = await _elasticSearchRepository.SearchAsync<CourseElasticDto>(elasticQuery, filter);

        var courseIds = courseDtos.Select(x => x.Id).ToArray();
        var query = _dbContext.Courses
            .Include(c => c.CourseDetails)
            .AsQueryable();

        query = query.Where(c => courseIds.Contains(c.Id));

        var courses = await query.ToArrayAsync();

        return courses;
    }

    //from elastic then pg
    public new async Task<IEnumerable<Domain.Entities.Course>> GetManyAsync(Expression<Func<Domain.Entities.Course, bool>> predicate, EndpointFilter filter)
    {
        var courseDtos = await _elasticSearchRepository.SearchAsync<CourseElasticDto>(predicate.ToString(), filter);

        var courseIds = courseDtos.Select(x => x.Id).ToArray();

        var query = _dbContext.Courses
            .Include(c => c.CourseDetails)
            .AsQueryable();

        query = query.Where(c => courseIds.Contains(c.Id));

        return await query.ToArrayAsync();
    }

    // add elastic and pg
    public new async Task<Guid> AddAsync(Domain.Entities.Course entity)
    {
        var courseDto = entity.ToCourseElasticDto();

        await Task.WhenAll(
            base.AddAsync(entity),
            _elasticSearchRepository.IndexAsync(courseDto)
        );
        
        return entity.Id;
    }

    // add elastic and pg
    public new async Task<Guid[]> AddManyAsync(IEnumerable<Domain.Entities.Course> entities)
    {
        var courses = entities as Domain.Entities.Course[] ?? entities.ToArray();
        if (!courses.Any()) return [];

        var courseDtos = courses.Select(c => c.ToCourseElasticDto()).ToArray();

        await Task.WhenAll(
            _elasticSearchRepository.BulkIndexAsync<CourseElasticDto>(courseDtos),
            _dbContext.Courses.AddRangeAsync(courses)
        );

        return courseDtos.Select(x => x.Id).ToArray();
    }

    // update elastic and pg
    public new async Task<Guid> UpdateAsync(Domain.Entities.Course entity)
    {
        await Task.WhenAll(
            base.UpdateAsync(entity),
            _elasticSearchRepository.UpdateAsync(entity.Id.ToString(), entity.ToCourseElasticDto())
        );

        return entity.Id;
    }

    // update elastic and pg
    public new  async Task<Guid> UpdateAsync(Domain.Entities.Course entity, Dictionary<string, object> updatedValues)
    {
        var updated = await base.UpdateAsync(entity, updatedValues);

        await _elasticSearchRepository.UpdateAsync(entity.Id.ToString(), updated.ToCourseElasticDto());

        return entity.Id;
    }

    // update elastic and pg
    public new async Task<Guid[]> UpdateManyAsync(IEnumerable<Domain.Entities.Course> entities)
    {
        var courses = entities as Domain.Entities.Course[] ?? entities.ToArray();
        if (!courses.Any()) return [];
        var courseDtos = courses.Select(c => c.ToCourseElasticDto()).ToArray();

        await Task.WhenAll(
            base.UpdateManyAsync(courses),
            _elasticSearchRepository.BulkIndexAsync<CourseElasticDto>(courseDtos)
        );

        return courseDtos.Select(x => x.Id).ToArray();
    }

    // delete elastic and pg
    public new async Task<Guid> DeleteAsync(Domain.Entities.Course entity)
    {
        await Task.WhenAll(
            base.DeleteAsync(entity),
            _elasticSearchRepository.DeleteAsync<CourseElasticDto>(entity.Id.ToString())
        );
        return entity.Id;
    }

    // delete elastic and pg
    public new async Task<Guid[]> DeleteManyAsync(IEnumerable<Domain.Entities.Course> entities)
    {
        var courses = entities as Domain.Entities.Course[] ?? entities.ToArray();
        if (!courses.Any()) return [];
        var ids = courses.Select(e => e.Id.ToString());

        await Task.WhenAll(
            base.DeleteManyAsync(courses),
            _elasticSearchRepository.BulkDeleteAsync<CourseElasticDto>(ids)
        );

        return courses.Select(x => x.Id).ToArray();
    }
}