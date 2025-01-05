using Iyzipay.Model;

namespace Udemy.Payment.Domain.Dtos;

public class CancelRequest
{
    public string PaymentId { get; set; }
    public string ConversationId { get; set; }
    public RefundReason CancelReason { get; set; }
}