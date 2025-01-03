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

    public async Task<LessonCategory?> GetByIdAsync(Guid id, Guid courseId)
    {
        var lessonCategory = await _context.LessonCategories.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id && x.CourseId == courseId);

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

    public async Task<Guid> AddAsync(LessonCategory entity, Guid courseId)
    {
        entity.CourseId = courseId;

        await _context.LessonCategories.AddAsync(entity);

        await _context.SaveChangesAsync();

        return entity.Id;
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