using Microsoft.EntityFrameworkCore;
using Udemy.Common.Base;
using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Entities;
using Udemy.Course.Domain.Interfaces.Repository;
using Udemy.Course.Infrastructure.Contexts;

namespace Udemy.Course.Infrastructure.Repositories;

public class CommentRepository(ApplicationDbContext context) : BaseRepository<Comment>(context), ICommentRepository
{
    private readonly ApplicationDbContext _context = context;

    public override async Task<Comment?> GetByIdAsync(Guid id)
    {
        var comment = await _context.Comments
            .AsNoTracking()
            .Include(x => x.Rate)
            .FirstOrDefaultAsync(x => x.Id == id);

        return comment;
    }

    public async Task<IEnumerable<Comment>> GetCommentsByCourseIdAsync(Guid courseId, EndpointFilter filter)
    {
        var query = _context.Comments
            .AsNoTracking()
            .Include(x => x.Rate)
            .Where(x => x.CourseId == courseId);

        query = Filtering(query, filter);

        query = Sorting(query, filter);

        query = Paginating(query, filter);

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(Guid userId, EndpointFilter filter)
    {
        var query = _context.Comments
            .AsNoTracking()
            .Include(x => x.Rate)
            .Where(x => x.UserId == userId);

        query = Filtering(query, filter);

        query = Sorting(query, filter);

        query = Paginating(query, filter);

        return await query.ToListAsync();
    }

    public override async Task<Guid> AddAsync(Comment entity)
    {
        await _context.Comments.AddAsync(entity);

        var courseDetails = await _context.Courses
            .Include(x => x.CourseDetails)
            .Select(x => x.CourseDetails)
            .FirstOrDefaultAsync(x => x!.Id == entity.CourseId);

        courseDetails!.RateCount += 1;
        courseDetails!.RateValue += entity.Rate!.Value!;

        await _context.SaveChangesAsync();

        return entity.Id;
    }

    public override async Task<Comment> UpdateAsync(Comment entity)
    {
        _context.Comments.Update(entity);

        var courseDetails = await _context.Courses
            .Include(x => x.CourseDetails)
            .Select(x => x.CourseDetails)
            .FirstOrDefaultAsync(x => x!.Id == entity.CourseId);

        courseDetails!.RateValue += entity.Rate!.Value!;

        await _context.SaveChangesAsync();

        return entity;
    }

    public override async Task<Guid> DeleteAsync(Comment entity)
    {
        _context.Comments.Remove(entity);

        var courseDetails = await _context.Courses
            .Include(x => x.CourseDetails)
            .Select(x => x.CourseDetails)
            .FirstOrDefaultAsync(x => x!.Id == entity.CourseId);

        courseDetails!.RateCount -= 1;
        courseDetails!.RateValue -= entity.Rate!.Value!;

        await _context.SaveChangesAsync();

        return entity.Id;
    }
}