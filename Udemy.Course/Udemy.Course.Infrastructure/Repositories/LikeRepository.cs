using Microsoft.EntityFrameworkCore;
using Udemy.Common.Base;
using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Entities;
using Udemy.Course.Domain.Interfaces.Repository;
using Udemy.Course.Infrastructure.Contexts;

namespace Udemy.Course.Infrastructure.Repositories;

public class LikeRepository(ApplicationDbContext context) : BaseRepository<Like>(context), ILikeRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<Like>> GetLikesByQuestionIdAsync(Guid questionId, EndpointFilter filter)
    {
        var query = _context.Likes
            .AsNoTracking()
            .Where(x => x.QuestionId == questionId)
            .AsQueryable();

        query = Filtering(query, filter);

        query = Sorting(query, filter);

        query = Paginating(query, filter);

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Like>> GetLikesByAnswerIdAsync(Guid answerId, EndpointFilter filter)
    {
        var query = _context.Likes
            .AsNoTracking()
            .Where(x => x.AnswerId == answerId)
            .AsQueryable();

        query = Filtering(query, filter);

        query = Sorting(query, filter);

        query = Paginating(query, filter);

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Like>> GetLikesByUserIdAsync(Guid userId, EndpointFilter filter)
    {
        var query = _context.Likes
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .AsQueryable();

        query = Filtering(query, filter);

        query = Sorting(query, filter);

        query = Paginating(query, filter);

        return await query.ToListAsync();
    }

    public async Task<Guid> AddAsync(Guid answerId, Like like)
    {
        await _context.Likes.AddAsync(like);
        await _context.SaveChangesAsync();

        return like.Id;
    }

    public async Task DeleteAsync(Guid answerId, Guid likeId)
    {
        var like = await _context.Likes
            .FirstOrDefaultAsync(x => x.AnswerId == answerId && x.Id == likeId);

        if (like is null)
        {
            throw new KeyNotFoundException();
        }

        _context.Likes.Remove(like);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsLiked(Guid userId, Guid answerId)
    {
        return await _context.Likes
            .AnyAsync(x => x.UserId == userId && x.AnswerId == answerId);
    }

    public async Task<Like> GetLikeByIdAsync(Guid userId, Guid likeId)
    {
        var like = await _context.Likes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == userId && x.Id == likeId);

        return like ?? throw new KeyNotFoundException();
    }

    public async Task<Like> GetLikeByUserIdAsync(Guid userId, Guid answerId)
    {
        var like = await _context.Likes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == userId && x.AnswerId == answerId);

        return like ?? throw new KeyNotFoundException();
    }
}