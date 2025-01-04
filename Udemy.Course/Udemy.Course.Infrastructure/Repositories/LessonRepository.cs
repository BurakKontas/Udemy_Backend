using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Udemy.Common.Base;
using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Entities;
using Udemy.Course.Domain.Interfaces.Repository;
using Udemy.Course.Infrastructure.Contexts;

namespace Udemy.Course.Infrastructure.Repositories;

public class LessonRepository(ApplicationDbContext context) : BaseRepository<Lesson>(context), ILessonRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<Lesson>> GetAll(Guid categoryId, EndpointFilter filter)
    {
        var query = _context.Lessons.AsNoTracking()
            .Where(x => x.LessonCategoryId == categoryId);

        // Filtering
        query = Filtering(query, filter);

        // Sorting
        query = Sorting(query, filter);

        // Paginating
        query = Paginating(query, filter);

        return await query.ToListAsync();
    }

    public async Task<Lesson?> GetByIdAsync(Guid id, Guid categoryId)
    {
        var lesson = await _context.Lessons.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id && x.LessonCategoryId == categoryId);

        return lesson;
    }

    public async Task<IEnumerable<Lesson>> GetManyAsync(Expression<Func<Lesson, bool>> predicate, Guid categoryId, EndpointFilter filter)
    {
        var query = _context.Lessons.AsNoTracking()
            .Where(x => x.LessonCategoryId == categoryId)
            .Where(predicate);

        // Filtering
        query = Filtering(query, filter);

        // Sorting
        query = Sorting(query, filter);

        // Paginating
        query = Paginating(query, filter);

        return await query.ToListAsync();
    }

    public async Task<Guid> AddAsync(Guid userId, Lesson entity, Guid categoryId)
    {
        entity.LessonCategoryId = categoryId;

        var courseId = await _context.LessonCategories
            .AsNoTracking()
            .Select(x => x.CourseId)
            .FirstOrDefaultAsync(x => x == categoryId);

        var courseDetails = await _context.Courses
            .Include(x => x.CourseDetails)
            .Select(x => x.CourseDetails)
            .FirstOrDefaultAsync(x => x!.Id == courseId);

        if(courseDetails is null)
            throw new ArgumentNullException($"Course not found");

        var instructors = await _context.Courses
            .AsNoTracking()
            .Where(x => x.Id == courseId)
            .Select(x => x.InstructorIds)
            .FirstOrDefaultAsync();

        if (instructors is null)
            throw new ArgumentNullException($"Instructors not found");

        if (!instructors.Contains(userId))
            throw new UnauthorizedAccessException($"User is not authorized to add lesson to this course");
        
        entity.CourseId = courseDetails.CourseId;

        await _context.Lessons.AddAsync(entity);

        courseDetails.TotalDuration += entity.Duration;

        await _context.SaveChangesAsync();

        return entity.Id;
    }

    public async Task<Guid> UpdateAsync(Guid userId, Lesson entity, Dictionary<string, object> updates)
    {
        var instructors = await _context.Courses
            .AsNoTracking()
            .Where(x => x.Id == entity.CourseId)
            .Select(x => x.InstructorIds)
            .FirstOrDefaultAsync();

        if (instructors is null)
            throw new ArgumentNullException($"Instructors not found");

        if (!instructors.Contains(userId))
            throw new UnauthorizedAccessException($"User is not authorized to update lesson to this course");

        await base.UpdateAsync(entity, updates);

        return entity.Id;
    }

    public async Task<Guid> DeleteAsync(Guid userId, Guid id)
    {
        var instructors = await _context.Courses
            .AsNoTracking()
            .Where(x => x.Lessons.Any(x => x.Id == id))
            .Select(x => x.InstructorIds)
            .FirstOrDefaultAsync();

        if (instructors is null)
            throw new ArgumentNullException($"Instructors not found");

        if (!instructors.Contains(userId))
            throw new UnauthorizedAccessException($"User is not authorized to delete lesson to this course");

        var lesson = await GetByIdAsync(id);

        if (lesson is null)
            throw new ArgumentNullException($"Lesson not found");

        await base.DeleteAsync(lesson);

        return id;
    }

    public override async Task<Guid> DeleteAsync(Lesson lesson)
    {
        var courseDetails = await _context.Courses
            .Include(x => x.CourseDetails)
            .Select(x => x.CourseDetails)
            .FirstOrDefaultAsync(x => x!.Id == lesson.CourseId);

        if (courseDetails is null)
            throw new ArgumentNullException($"Course not found");

        courseDetails.TotalDuration -= lesson.Duration;

        _context.Lessons.Remove(lesson);

        await _context.SaveChangesAsync();

        return lesson.Id;
    }
}