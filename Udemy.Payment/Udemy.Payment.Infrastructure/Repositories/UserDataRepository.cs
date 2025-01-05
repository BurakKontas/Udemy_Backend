using Microsoft.EntityFrameworkCore;
using Udemy.Common.Base;
using Udemy.Payment.Domain.Entities;
using Udemy.Payment.Domain.Interfaces;
using Udemy.Payment.Infrastructure.Contexts;

namespace Udemy.Payment.Infrastructure.Repositories;

public class UserDataRepository(ApplicationDbContext context) : BaseRepository<UserDataEntity>(context), IUserDataRepository
{
    private readonly ApplicationDbContext _context = context;


    public async Task<IEnumerable<UserDataEntity>> GetAllAsync()
    {
        var userData = await _context.UserDatas
            .ToListAsync();

        return userData;
    }

    public async Task CreateAsync(UserDataEntity userData)
    {
        await _context.UserDatas.AddAsync(userData);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid userId)
    {
        var userData = await GetByIdAsync(userId);

        if (userData is not null)
        {
            _context.UserDatas.Remove(userData);
        }

        await _context.SaveChangesAsync();
    }
}