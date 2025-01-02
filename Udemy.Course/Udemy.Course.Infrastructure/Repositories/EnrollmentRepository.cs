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

    public async Task<IEnumerable<Enrollment>> GetAll(Guid courseId, EndpointFilter filter)
    {
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

    public async Task<Enrollment?> GetByIdAsync(Guid id, Guid courseId)
    {
        var enrollment = await _context.Enrollments.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id && x.CourseId == courseId);

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

    public async Task<IEnumerable<Enrollment>> GetAllByCourseId(Guid courseId, EndpointFilter filter)
    {
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
}