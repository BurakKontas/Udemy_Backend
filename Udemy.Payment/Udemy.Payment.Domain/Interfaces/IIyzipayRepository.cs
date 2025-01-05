using Udemy.Payment.Domain.Dtos;

namespace Udemy.Payment.Domain.Interfaces;

public interface IIyzipayRepository
{
    Task<PaymentResponse> CreatePaymentAsync(PaymentRequest request);
    Task<PaymentDetailResponse> GetPaymentDetailsAsync(string paymentId);
    Task<RefundResponse> RefundPaymentAsync(RefundRequest request);
    Task<CardStoreResponse> StoreCardAsync(CardStoreRequest request);
    Task<PaymentResponse> CreatePaymentWithStoredCardAsync(StoredCardPaymentRequest request);
    Task<ThreeDSecureResponse> InitializeThreeDSecureAsync(ThreeDSecureRequest request);
    Task<ThreeDSecureResponse> CompleteThreeDSecureAsync(string paymentId);
}
