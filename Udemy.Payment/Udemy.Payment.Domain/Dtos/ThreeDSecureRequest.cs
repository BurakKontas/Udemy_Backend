using Iyzipay.Model;

namespace Udemy.Payment.Domain.Dtos;

public class ThreeDSecureRequest
{
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string BasketId { get; set; }
    public Buyer Buyer { get; set; }
    public Address BillingAddress { get; set; }
    public Address ShippingAddress { get; set; }
    public List<BasketItem> BasketItems { get; set; }
    public PaymentCard PaymentCard { get; set; }
    public string CallbackUrl { get; set; }
}