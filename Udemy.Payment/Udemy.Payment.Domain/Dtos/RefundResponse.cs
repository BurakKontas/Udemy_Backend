namespace Udemy.Payment.Domain.Dtos;

public class RefundResponse
{
    public required string RefundId { get; set; }
    public required string Status { get; set; }
    public required string ErrorMessage { get; set; }
    public DateTime RefundDate { get; set; }
}