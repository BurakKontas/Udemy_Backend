using Iyzipay.Model;

namespace Udemy.Payment.Domain.Dtos;

public class PaymentRequest
{
    public required decimal Price { get; set; }
    public required string Currency { get; set; }
    public required Buyer Buyer { get; set; }
    public required Address BillingAddress { get; set; }
    public required Address ShippingAddress { get; set; }
    public required List<BasketItem> BasketItems { get; set; }
    public required PaymentCard PaymentCard { get; set; }
    public int Installment { get; set; }
}