using Udemy.Common.Base;

namespace Udemy.Payment.Domain.Entities;

public class BasketEntity : BaseEntity
{
    public required string Id { get; set; }
    public required decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public required ICollection<BasketItemEntity> Items { get; set; }
}