using Udemy.Payment.Domain.Enums;

namespace Udemy.Payment.Domain.Dtos;

public class ThreeDSecureResponse
{
    public string PaymentId { get; set; }
    public string HtmlContent { get; set; }
    public PaymentStatus Status { get; set; }
    public string ErrorMessage { get; set; }
    public string BasketId { get; set; }
}