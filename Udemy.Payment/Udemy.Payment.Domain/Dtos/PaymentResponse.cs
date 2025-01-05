namespace Udemy.Payment.Domain.Dtos;

public class PaymentResponse
{
    public string PaymentId { get; set; }
    public string Status { get; set; }
    public string ErrorMessage { get; set; }
    public DateTime TransactionDate { get; set; }
}