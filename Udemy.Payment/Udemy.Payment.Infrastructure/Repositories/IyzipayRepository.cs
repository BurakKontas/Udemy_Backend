using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.Extensions.Logging;
using Udemy.Payment.Domain.Dtos;
using Udemy.Payment.Domain.Interfaces;
using Udemy.Payment.Domain.Utils;
using IyzipayOptions = Iyzipay.Options;
using IyzipayPayment = Iyzipay.Model.Payment;

namespace Udemy.Payment.Infrastructure.Repositories
{
    public class IyzipayRepository(IyzipayOptions options, ILogger<IyzipayRepository> logger) : IIyzipayRepository
    {
        private readonly IyzipayOptions _options = options;
        private readonly ILogger<IyzipayRepository> _logger = logger;

        public async Task<PaymentResponse> CreatePaymentAsync(PaymentRequest request)
        {
            _logger.LogInformation("Creating payment for BasketId: {BasketId} with amount: {Amount}", request.BasketId, request.Amount);

            var createPaymentRequest = IyzipayUtils.CreatePaymentRequest(
                conversationId: IyzipayUtils.GenerateId("PAYMENT_CONV"),
                price: request.Amount,
                basketId: request.BasketId,
                paymentChannel: PaymentChannel.WEB,
                paymentGroup: PaymentGroup.PRODUCT,
                buyer: request.Buyer,
                billingAddress: request.BillingAddress,
                shippingAddress: request.ShippingAddress,
                basketItems: request.BasketItems,
                paymentCard: request.PaymentCard,
                installment: request.Installment
            );

            try
            {
                var payment = await IyzipayPayment.Create(createPaymentRequest, _options);
                _logger.LogInformation("Payment created successfully, PaymentId: {PaymentId}, Status: {Status}", payment.PaymentId, payment.Status);

                return new PaymentResponse
                {
                    PaymentId = payment.PaymentId,
                    Status = payment.Status,
                    ErrorMessage = payment.ErrorMessage,
                    TransactionDate = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating payment for BasketId: {BasketId}", request.BasketId);
                throw;
            }
        }

        public async Task<PaymentDetailResponse> GetPaymentDetailsAsync(string paymentId)
        {
            var request = new RetrievePaymentRequest
            {
                PaymentId = paymentId,
                Locale = Locale.TR.ToString(),
                ConversationId = IyzipayUtils.GenerateId("DETAIL_CONV")
            };

            try
            {
                var payment = await IyzipayPayment.Retrieve(request, _options);
                return new PaymentDetailResponse
                {
                    PaymentId = payment.PaymentId,
                    Status = payment.Status,
                    Amount = Convert.ToDecimal(payment.PaidPrice),
                    Currency = payment.Currency,
                    PaymentDate = DateTime.UtcNow,
                    BasketItems = payment.PaymentItems?.Select(item => new BasketItem
                    {
                        Id = item.ItemId,
                        Name = "Undefined",
                        Price = item.Price
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving payment details for PaymentId: {PaymentId}", paymentId);
                throw;
            }
        }

        public async Task<RefundResponse> RefundPaymentAsync(RefundRequest request)
        {
            var refundRequest = new CreateRefundRequest
            {
                PaymentTransactionId = request.PaymentId,
                Locale = Locale.TR.ToString(),
                ConversationId = IyzipayUtils.GenerateId("REFUND_CONV"),
                Price = request.RefundAmount.ToString("F2"),
                Currency = Currency.TRY.ToString()
            };

            try
            {
                var refund = await Refund.Create(refundRequest, _options);
                return new RefundResponse
                {
                    RefundId = refund.PaymentId,
                    Status = refund.Status,
                    ErrorMessage = refund.ErrorMessage,
                    RefundDate = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while refunding payment for PaymentId: {PaymentId}", request.PaymentId);
                throw;
            }
        }

        public async Task<CardStoreResponse> StoreCardAsync(CardStoreRequest request)
        {
            var externalId = IyzipayUtils.GenerateId("CARD_EXT");
            var cardRequest = new CreateCardRequest
            {
                Locale = Locale.TR.ToString(),
                ConversationId = IyzipayUtils.GenerateId("CARD_CONV"),
                Card = new CardInformation()
                {
                    CardHolderName = request.CardHolderName,
                    CardNumber = request.CardNumber,
                    ExpireMonth = request.ExpireMonth,
                    ExpireYear = request.ExpireYear,
                    CardAlias = request.UserKey
                },
                CardUserKey = request.UserKey,
                ExternalId = externalId,
                Email = request.Email ?? string.Empty
            };

            try
            {
                var card = await Card.Create(cardRequest, _options);
                return new CardStoreResponse
                {
                    CardToken = card.CardToken,
                    CardAlias = card.CardAlias,
                    Status = card.Status,
                    ErrorMessage = card.ErrorMessage,
                    ExternalId = card.ExternalId,
                    Email = card.Email
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while storing card for UserKey: {UserKey}", request.UserKey);
                throw;
            }
        }

        public async Task<PaymentResponse> CreatePaymentWithStoredCardAsync(StoredCardPaymentRequest request)
        {
            var createPaymentRequest = new CreatePaymentRequest
            {
                Locale = Locale.TR.ToString(),
                ConversationId = IyzipayUtils.GenerateId("STORED_PAYMENT_CONV"),
                Price = request.Amount.ToString("F2"),
                PaidPrice = request.Amount.ToString("F2"),
                Currency = Currency.TRY.ToString(),
                Installment = request.Installment,
                BasketId = request.BasketId,
                PaymentCard = new PaymentCard
                {
                    CardToken = request.CardToken,
                    CardUserKey = request.UserKey
                },
                Buyer = request.Buyer,
                BillingAddress = request.BillingAddress,
                ShippingAddress = request.ShippingAddress,
                BasketItems = request.BasketItems
            };

            try
            {
                var payment = await IyzipayPayment.Create(createPaymentRequest, _options);
                return new PaymentResponse
                {
                    PaymentId = payment.PaymentId,
                    Status = payment.Status,
                    ErrorMessage = payment.ErrorMessage,
                    TransactionDate = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating payment with stored card for BasketId: {BasketId}", request.BasketId);
                throw;
            }
        }

        public async Task<ThreeDSecureResponse> InitializeThreeDSecureAsync(ThreeDSecureRequest request)
        {
            var threeDsRequest = new CreatePaymentRequest
            {
                Locale = Locale.TR.ToString(),
                ConversationId = IyzipayUtils.GenerateId("3DS_INIT_CONV"),
                Price = request.Amount.ToString("F2"),
                PaidPrice = request.Amount.ToString("F2"),
                CallbackUrl = request.CallbackUrl,
                PaymentCard = request.PaymentCard,
                Buyer = request.Buyer,
                BillingAddress = request.BillingAddress,
                ShippingAddress = request.ShippingAddress,
                BasketItems = request.BasketItems
            };

            try
            {
                var threeDSecure = await ThreedsInitialize.Create(threeDsRequest, _options);
                return new ThreeDSecureResponse
                {
                    PaymentId = IyzipayUtils.GenerateId("3DS_PAYMENT"),
                    HtmlContent = threeDSecure.HtmlContent,
                    Status = threeDSecure.Status,
                    ErrorMessage = threeDSecure.ErrorMessage
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while initializing 3D Secure for BasketId: {BasketId}", request.BasketId);
                throw;
            }
        }

        public async Task<ThreeDSecureResponse> CompleteThreeDSecureAsync(string paymentId)
        {
            var request = new RetrievePaymentRequest
            {
                Locale = Locale.TR.ToString(),
                PaymentId = paymentId
            };

            try
            {
                var payment = await IyzipayPayment.Retrieve(request, _options);
                return new ThreeDSecureResponse
                {
                    PaymentId = payment.PaymentId,
                    Status = payment.Status,
                    ErrorMessage = payment.ErrorMessage
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while completing 3D Secure for PaymentId: {PaymentId}", paymentId);
                throw;
            }
        }
    }
}
