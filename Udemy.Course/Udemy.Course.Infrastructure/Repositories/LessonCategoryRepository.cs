using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Udemy.Common.Base;
using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Entities;
using Udemy.Course.Domain.Interfaces.Repository;
using Udemy.Course.Infrastructure.Contexts;

namespace Udemy.Course.Infrastructure.Repositories;

public class LessonCategoryRepository(ApplicationDbContext context) : BaseRepository<LessonCategory>(context), ILessonCategoryRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<LessonCategory>> GetAll(Guid courseId, EndpointFilter filter)
    {
        var query = _context.LessonCategories.AsNoTracking()
            .Where(x => x.CourseId == courseId);

        // Filtering
        query = Filtering(query, filter);

        // Sorting
        query = Sorting(query, filter);

        // Paginating
        query = Paginating(query, filter);

        return await query.ToListAsync();
    }

    public async Task<LessonCategory?> GetByIdAsync(Guid userId, Guid categoryId)
    {
        var lessonCategory = await _context.LessonCategories.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == categoryId);

        if(lessonCategory is null)
        {
            return null;
        }

        var instructors = await _context.Courses.AsNoTracking()
            .Where(x => x.Id == lessonCategory.CourseId)
            .Select(x => x.InstructorIds)
            .FirstOrDefaultAsync();

        if(instructors is null)
        {
            return null;
        }

        if (!instructors.Contains(userId))
        {
            throw new UnauthorizedAccessException("You cannot access category that you doesn't own.");
        }

        return lessonCategory;
    }

    public async Task<IEnumerable<LessonCategory>> GetManyAsync(Expression<Func<LessonCategory, bool>> predicate, Guid courseId, EndpointFilter filter)
    {
        var query = _context.LessonCategories.AsNoTracking()
            .Where(x => x.CourseId == courseId)
            .Where(predicate);

        // Filtering
        query = Filtering(query, filter);

        // Sorting
        query = Sorting(query, filter);

        // Paginating
        query = Paginating(query, filter);

        return await query.ToListAsync();
    }

    public async Task<Guid> AddAsync(Guid userId, LessonCategory entity, Guid courseId)
    {
        var instructors = await _context.Courses.AsNoTracking()
            .Where(x => x.Id == courseId)
            .Select(x => x.InstructorIds)
            .FirstOrDefaultAsync();

        if(instructors is null)
        {
            throw new KeyNotFoundException("Course not found.");
        }

        if (!instructors.Contains(userId))
        {
            throw new UnauthorizedAccessException("You cannot add category to course that you doesn't own.");
        }

        entity.CourseId = courseId;

        await _context.LessonCategories.AddAsync(entity);

        await _context.SaveChangesAsync();

        return entity.Id;
    }

    public async Task<LessonCategory> UpdateAsync(Guid userId, LessonCategory entity, Dictionary<string, object> updatedValues)
    {
        var instructors = await _context.Courses.AsNoTracking()
            .Where(x => x.Id == entity.CourseId)
            .Select(x => x.InstructorIds)
            .FirstOrDefaultAsync();

        if(instructors is null)
        {
            throw new KeyNotFoundException("Course not found.");
        }

        if(!instructors.Contains(userId))
        {
            throw new UnauthorizedAccessException("You cannot update category that you doesn't own.");
        }

        return await base.UpdateAsync(entity, updatedValues);
    }

    public async Task<Guid> DeleteAsync(Guid userId, LessonCategory entity)
    {
        var instructors = await _context.Courses.AsNoTracking()
            .Where(x => x.Id == entity.CourseId)
            .Select(x => x.InstructorIds)
            .FirstOrDefaultAsync();

        if(instructors is null)
        {
            throw new KeyNotFoundException("Course not found.");
        }

        if(!instructors.Contains(userId))
        {
            throw new UnauthorizedAccessException("You cannot delete category that you doesn't own.");
        }

        return await base.DeleteAsync(entity);
    }

    public async Task<IEnumerable<LessonCategory>> GetByCourseIdAsync(Guid courseId, EndpointFilter filter)
    {
        var query = _context.LessonCategories.AsNoTracking()
            .Where(x => x.CourseId == courseId);

        // Filtering
        query = Filtering(query, filter);

        // Sorting
        query = Sorting(query, filter);

        // Paginating
        query = Paginating(query, filter);

        return await query.ToListAsync();
    }
}