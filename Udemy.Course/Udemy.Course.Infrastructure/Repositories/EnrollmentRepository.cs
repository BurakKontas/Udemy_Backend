using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Udemy.Common.Base;
using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Entities;
using Udemy.Course.Domain.Interfaces.Repository;
using Udemy.Course.Infrastructure.Contexts;

namespace Udemy.Course.Infrastructure.Repositories;

public class EnrollmentRepository(ApplicationDbContext context) : BaseRepository<Enrollment>(context), IEnrollmentRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<Enrollment>> GetAllAsync(Guid userId, Guid courseId, EndpointFilter filter)
    {
        var instructorIds = await _context.Courses.AsNoTracking()
            .Where(x => x.Id == courseId)
            .Select(x => x.InstructorIds)
            .FirstOrDefaultAsync();

        if (instructorIds is null)
        {
            throw new KeyNotFoundException("Course not found.");
        }

        if (!instructorIds.Contains(userId))
        {
            throw new UnauthorizedAccessException("You cannot access enrollment list of course you doesn't own.");
        }

        var query = _context.Enrollments.AsNoTracking()
            .Where(x => x.CourseId == courseId);

        // Filtering
        query = Filtering(query, filter);

        // Sorting
        query = Sorting(query, filter);

        // Paginating
        query = Paginating(query, filter);

        return await query.ToListAsync();
    }

    public async Task<Enrollment?> GetByIdAsync(Guid consumerId, Guid enrollmentId)
    {
        var enrollment = await _context.Enrollments.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == enrollmentId);

        if (enrollment is null)
        {
            return null;
        }

        var instructorIds = await _context.Courses.AsNoTracking()
            .Where(x => x.Id == enrollment.CourseId)
            .Select(x => x.InstructorIds)
            .FirstOrDefaultAsync();

        if (instructorIds is null)
        {
            throw new KeyNotFoundException("Course not found.");
        }

        var isInstructor = instructorIds.Contains(consumerId);

        if (!isInstructor && enrollment.StudentId != consumerId)
        {
            throw new UnauthorizedAccessException("You cannot access enrollment of course you doesn't own.");
        }

        return enrollment;
    }

    public async Task<IEnumerable<Enrollment>> GetManyAsync(Expression<Func<Enrollment, bool>> predicate, Guid courseId, EndpointFilter filter)
    {
        var query = _context.Enrollments.AsNoTracking()
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

    public async Task<Guid> AddAsync(Enrollment entity, Guid courseId)
    {
        entity.CourseId = courseId;

        await _context.Enrollments.AddAsync(entity);

        await _context.SaveChangesAsync();

        return entity.Id;
    }

    public async Task<IEnumerable<Enrollment>> GetAllByCourseIdAsync(Guid userId, Guid courseId, EndpointFilter filter)
    {
        var instructorIds = await _context.Courses.AsNoTracking()
            .Where(x => x.Id == courseId)
            .Select(x => x.InstructorIds)
            .FirstOrDefaultAsync();

        if(instructorIds is null)
        {
            throw new KeyNotFoundException("Course not found.");
        }

        if(!instructorIds.Contains(userId))
        {
            throw new UnauthorizedAccessException("You cannot access enrollment list of course you doesn't own.");
        }

        var query = _context.Enrollments.AsNoTracking()
            .Where(x => x.CourseId == courseId);

        // Filtering
        query = Filtering(query, filter);

        // Sorting
        query = Sorting(query, filter);

        // Paginating
        query = Paginating(query, filter);

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Enrollment>> GetAllByUserIdAsync(Guid userId, EndpointFilter filter)
    {
        var query = _context.Enrollments.AsNoTracking()
            .Where(x => x.StudentId == userId);

        // Filtering
        query = Filtering(query, filter);

        // Sorting
        query = Sorting(query, filter);

        // Paginating
        query = Paginating(query, filter);

        return await query.ToListAsync();
    }
}