using Microsoft.EntityFrameworkCore;
using Udemy.Common.Base;
using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Entities;
using Udemy.Course.Domain.Interfaces.Repository;
using Udemy.Course.Infrastructure.Contexts;

namespace Udemy.Course.Infrastructure.Repositories;

public class FavoriteRepository(ApplicationDbContext context) : BaseRepository<Favorite>(context), IFavoriteRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<Favorite>> GetFavoritesByUserIdAsync(Guid userId, EndpointFilter filter)
    {
        var query = _context.Favorites
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .AsQueryable();

        query = Filtering(query, filter);

        query = Sorting(query, filter);

        query = Paginating(query, filter);

        return await query.ToListAsync();
        
    }

    public async Task<Guid> AddAsync(Guid userId, Favorite favorite)
    {
        favorite.UserId = userId;

        await _context.Favorites.AddAsync(favorite);

        await _context.SaveChangesAsync();

        return favorite.Id;
    }

    public async Task<Guid> DeleteAsync(Guid userId, Guid favoriteId)
    {
        var favorite = await _context.Favorites
            .FirstOrDefaultAsync(x => x.UserId == userId && x.Id == favoriteId);

        if (favorite is null)
        {
            throw new KeyNotFoundException();
        }

        _context.Favorites.Remove(favorite);

        await _context.SaveChangesAsync();

        return favorite.Id;
    }

    public async Task<bool> IsFavorite(Guid userId, Guid courseId)
    {
        return await _context.Favorites
            .AnyAsync(x => x.UserId == userId && x.CourseId == courseId);
    }

    public async Task<Favorite> GetFavoriteByIdAsync(Guid userId, Guid favoriteId)
    {
        var favorite = await _context.Favorites
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == userId && x.Id == favoriteId);

        return favorite ?? throw new KeyNotFoundException();
    }

    public async Task<Favorite> GetFavoriteByUserIdAsync(Guid userId, Guid courseId)
    {
        var favorite = await _context.Favorites
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == userId && x.CourseId == courseId);;

        return favorite ?? throw new KeyNotFoundException();
    }

    public async Task<IEnumerable<Favorite>> GetFavoritesByCourseIdAsync(Guid courseId, EndpointFilter filter)
    {
        var query = _context.Favorites
            .AsNoTracking()
            .Where(x => x.CourseId == courseId)
            .AsQueryable();

        query = Filtering(query, filter);

        query = Sorting(query, filter);

        query = Paginating(query, filter);

        return await query.ToListAsync();
    }

    public async Task<Favorite> GetFavoriteByUserIdAndCourseId(Guid userId, Guid courseId)
    {
        var favorite = await _context.Favorites
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == userId && x.CourseId == courseId);

        return favorite ?? throw new KeyNotFoundException();
    }
}