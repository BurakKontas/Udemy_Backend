using Iyzipay.Model;
using Udemy.Payment.Domain.Enums;

namespace Udemy.Payment.Domain.Dtos;

public class RefundResponse
{
    public required string RefundId { get; set; }
    public required string Status { get; set; }
    public required string ErrorMessage { get; set; }
    public DateTimeOffset RefundDate { get; set; }
    public RefundReason RefundReason { get; set; }
}