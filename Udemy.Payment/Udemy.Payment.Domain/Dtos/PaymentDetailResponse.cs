using Iyzipay.Model;
using Udemy.Payment.Domain.Enums;

namespace Udemy.Payment.Domain.Dtos;

public class PaymentDetailResponse
{
    public required string PaymentId { get; set; }
    public required PaymentStatus Status { get; set; }
    public decimal Amount { get; set; }
    public required string Currency { get; set; }
    public required DateTime PaymentDate { get; set; }
    public List<BasketItem>? BasketItems { get; set; }
}