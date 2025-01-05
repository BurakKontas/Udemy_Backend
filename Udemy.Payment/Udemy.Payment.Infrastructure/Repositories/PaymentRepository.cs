using Microsoft.EntityFrameworkCore;
using Udemy.Common.Base;
using Udemy.Payment.Domain.Entities;
using Udemy.Payment.Domain.Interfaces;
using Udemy.Payment.Infrastructure.Contexts;

namespace Udemy.Payment.Infrastructure.Repositories;

public class PaymentRepository(ApplicationDbContext context) : BaseRepository<PaymentEntity>(context), IPaymentRepository
{
    private readonly ApplicationDbContext _context = context;


    public async Task<IEnumerable<PaymentEntity>> GetAllAsync()
    {
        var payments = await _context.Payments
            .ToListAsync();

        return payments;
    }

    public async Task CreateAsync(PaymentEntity payment)
    {
        await _context.Payments.AddAsync(payment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid paymentId)
    {
        var payment = await GetByIdAsync(paymentId);

        if (payment is not null)
        {
            _context.Payments.Remove(payment);
        }

        await _context.SaveChangesAsync();
    }
}