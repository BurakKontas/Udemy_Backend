using Udemy.Common.Base;
using Udemy.Payment.Domain.Enums;
using Udemy.Payment.Domain.Utils;

namespace Udemy.Payment.Domain.Entities;

public class PaymentEntity : BaseEntity
{
    public string Id { get; set; } = IyzipayUtils.GenerateId("PAYMENT");
    public PaymentMethod PaymentMethod { get; set; }
    public decimal TotalAmount { get; set; } 
    public DateTimeOffset PaymentDate { get; set; } = DateTime.UtcNow;
    public string? Description { get; set; }
    public string? IpAddress { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public string? ErrorMessage { get; set; }
    public Guid UserDataId { get; set; }
    public string BasketId { get; set; }
    public string CardId { get; set; }

    public virtual UserDataEntity UserData { get; set; } = null!;
    public virtual BasketEntity Basket { get; set; } = null!;
    public virtual CardEntity? Card { get; set; } = null!;
}