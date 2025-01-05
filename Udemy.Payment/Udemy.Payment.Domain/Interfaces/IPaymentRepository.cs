using Udemy.Common.Base;
using Udemy.Payment.Domain.Entities;

namespace Udemy.Payment.Domain.Interfaces;

public interface IPaymentRepository : IBaseRepository<PaymentEntity>
{
    Task<IEnumerable<PaymentEntity>> GetAllAsync();
    Task CreateAsync(PaymentEntity payment);
    Task DeleteAsync(Guid paymentId);
}