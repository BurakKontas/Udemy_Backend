namespace Udemy.Payment.Domain.Dtos;

public class RefundRequest
{
    public string PaymentId { get; set; }
    public decimal RefundAmount { get; set; }
    public string Reason { get; set; }
}