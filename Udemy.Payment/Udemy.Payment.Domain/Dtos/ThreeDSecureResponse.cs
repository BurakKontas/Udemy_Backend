namespace Udemy.Payment.Domain.Dtos;

public class ThreeDSecureResponse
{
    public string PaymentId { get; set; }
    public string HtmlContent { get; set; }
    public string Status { get; set; }
    public string ErrorMessage { get; set; }
}