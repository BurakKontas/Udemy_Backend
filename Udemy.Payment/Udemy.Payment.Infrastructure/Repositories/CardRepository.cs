using Microsoft.EntityFrameworkCore;
using Udemy.Common.Base;
using Udemy.Payment.Domain.Entities;
using Udemy.Payment.Domain.Interfaces;
using Udemy.Payment.Infrastructure.Contexts;

namespace Udemy.Payment.Infrastructure.Repositories;

public class CardRepository(ApplicationDbContext context) : BaseRepository<CardEntity>(context), ICardRepository
{
    private readonly ApplicationDbContext _context = context;


    public async Task<CardEntity?> GetByIdAsync(string cardId)
    {
        var card = await _context.Cards
            .FirstOrDefaultAsync(x => x.Id == cardId);

        return card;
    }

    public async Task<IEnumerable<CardEntity>> GetAllAsync()
    {
        var cards = await _context.Cards
            .ToListAsync();

        return cards;
    }

    public async Task CreateAsync(CardEntity card)
    {
        await _context.Cards.AddAsync(card);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string cardId)
    {
        var card = await GetByIdAsync(cardId);

        if (card is not null)
        {
            _context.Cards.Remove(card);
        }

        await _context.SaveChangesAsync();
    }
}