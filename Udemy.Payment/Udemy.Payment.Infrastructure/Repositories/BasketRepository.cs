using Microsoft.EntityFrameworkCore;
using Udemy.Common.Base;
using Udemy.Payment.Domain.Entities;
using Udemy.Payment.Domain.Interfaces;
using Udemy.Payment.Infrastructure.Contexts;

namespace Udemy.Payment.Infrastructure.Repositories;

public class BasketRepository(ApplicationDbContext context) : BaseRepository<BasketEntity>(context), IBasketRepository
{
    private readonly ApplicationDbContext _context = context;


    public async Task<BasketEntity?> GetByIdAsync(string basketId)
    {
        var basket = await _context.Baskets
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == basketId);

        return basket;
    }

    public async Task<IEnumerable<BasketEntity>> GetAllAsync()
    {
        var baskets = await _context.Baskets
            .Include(x => x.Items)
            .ToListAsync();

        return baskets;
    }

    public async Task CreateAsync(BasketEntity basket)
    {
        await _context.Baskets.AddAsync(basket);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string basketId)
    {
        var basket = await GetByIdAsync(basketId);

        if (basket is not null)
        {
            _context.Baskets.Remove(basket);
        }

        await _context.SaveChangesAsync();
    }

}