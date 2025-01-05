namespace Udemy.Payment.Domain.Enums.Extensions;

public static class PaymentStatusExtension
{
    public static PaymentStatus GetPaymentStatus(string status)
    {
        return status.ToLower() switch
        {
            "success" => PaymentStatus.Success,
            "pending" => PaymentStatus.Pending,
            "successful" => PaymentStatus.Successful,
            "failure" => PaymentStatus.Failure,
            "refunded" => PaymentStatus.Refunded,
            "canceled" => PaymentStatus.Canceled,
            "underreview" => PaymentStatus.UnderReview,
            "chargedback" => PaymentStatus.ChargedBack,
            _ => throw new ArgumentOutOfRangeException($"Unknown payment status: {status}")
        };
    }
}