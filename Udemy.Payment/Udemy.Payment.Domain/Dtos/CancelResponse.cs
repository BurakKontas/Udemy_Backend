using Iyzipay.Model;

namespace Udemy.Payment.Domain.Dtos;

public class CancelResponse
{
    public string CancelId { get; set; }
    public string Status { get; set; }
    public string ErrorMessage { get; set; }
    public DateTime CancelDate { get; set; }
    public RefundReason CancelReason { get; set; }
}