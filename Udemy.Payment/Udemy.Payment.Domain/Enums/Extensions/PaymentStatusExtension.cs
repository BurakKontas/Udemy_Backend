namespace Udemy.Payment.Domain.Enums.Extensions;

public static class PaymentStatusExtension
{
    public static PaymentStatus GetPaymentStatus(string status)
    {
        return status.ToLower() switch
        {
            "pending" => PaymentStatus.Pending,
            "successful" => PaymentStatus.Successful,
            "failed" => PaymentStatus.Failed,
            "refunded" => PaymentStatus.Refunded,
            "canceled" => PaymentStatus.Canceled,
            "underreview" => PaymentStatus.UnderReview,
            "chargedback" => PaymentStatus.ChargedBack,
            _ => throw new ArgumentOutOfRangeException($"Unknown payment status: {status}")
        };
    }
}