using Iyzipay.Model;

namespace Udemy.Payment.Domain.Dtos;

public class StoredCardPaymentRequest
{
    public string CardToken { get; set; }
    public string UserKey { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string BasketId { get; set; }
    public Buyer Buyer { get; set; }
    public Address BillingAddress { get; set; }
    public Address ShippingAddress { get; set; }
    public List<BasketItem> BasketItems { get; set; }
    public int Installment { get; set; }
}