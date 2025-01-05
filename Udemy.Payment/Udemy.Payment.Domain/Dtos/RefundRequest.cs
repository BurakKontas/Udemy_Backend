
using Iyzipay.Model;

namespace Udemy.Payment.Domain.Dtos;

public class RefundRequest
{
    public string PaymentTransactionId { get; set; }
    public decimal RefundAmount { get; set; }
    public RefundReason Reason { get; set; }
    public string ConversationId { get; set; }
}