﻿using System.Globalization;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.Extensions.Logging;
using Udemy.Payment.Domain.Dtos;
using Udemy.Payment.Domain.Entities;
using Udemy.Payment.Domain.Enums.Extensions;
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
            var basketId = IyzipayUtils.GenerateId("BASKET");

            _logger.LogInformation("Creating payment for BasketId: {BasketId} with amount: {Amount}", basketId, request.Price);

            var createPaymentRequest = IyzipayUtils.CreatePaymentRequest(
                conversationId: IyzipayUtils.GenerateId("PAYMENT_CONV"),
                price: request.Price,
                basketId: basketId,
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
                    Status = PaymentStatusExtension.GetPaymentStatus(payment.Status),
                    ErrorMessage = payment.ErrorMessage,
                    TransactionDate = DateTime.UtcNow,
                    BasketId = basketId,
                    Card = CardEntity.MapToCardEntity(payment),
                    ConversationId = payment.ConversationId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating payment for BasketId: {BasketId}", basketId);
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
                    Status = PaymentStatusExtension.GetPaymentStatus(payment.Status),
                    Amount = Convert.ToDecimal(payment.PaidPrice),
                    Currency = payment.Currency,
                    PaymentDate = DateTime.UtcNow,
                    BasketItems = IyzipayUtils.CreateBasket(payment.PaymentItems),
                    ConversationId = payment.ConversationId,
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
                PaymentTransactionId = request.PaymentTransactionId,
                Locale = Locale.TR.ToString(),
                ConversationId = request.ConversationId,
                Price = request.RefundAmount.ToString(CultureInfo.InvariantCulture),
                Currency = Currency.TRY.ToString(),
                Reason = request.Reason.ToString()
            };

            try
            {
                var refund = await Refund.Create(refundRequest, _options);
                return new RefundResponse
                {
                    RefundId = refund.PaymentId,
                    Status = refund.Status,
                    ErrorMessage = refund.ErrorMessage,
                    RefundDate = DateTime.UtcNow,
                    RefundReason = request.Reason
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while refunding payment for PaymentId: {PaymentId}", request.PaymentTransactionId);
                throw;
            }
        }

        public async Task<CancelResponse> CancelPaymentAsync(CancelRequest request)
        {
            var cancelRequest = new CreateCancelRequest
            {
                PaymentId = request.PaymentId,
                Locale = Locale.TR.ToString(),
                ConversationId = request.ConversationId,
                Reason = request.CancelReason.ToString()
            };

            try
            {
                var cancel = await Cancel.Create(cancelRequest, _options);
                return new CancelResponse
                {
                    CancelId = cancel.PaymentId,
                    Status = cancel.Status,
                    ErrorMessage = cancel.ErrorMessage,
                    CancelDate = DateTime.UtcNow,
                    CancelReason = request.CancelReason
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while cancelling payment for PaymentId: {PaymentId}", request.PaymentId);
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
                    Status = PaymentStatusExtension.GetPaymentStatus(payment.Status),
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
                    Status = PaymentStatusExtension.GetPaymentStatus(threeDSecure.Status),
                    ErrorMessage = threeDSecure.ErrorMessage,
                    BasketId = threeDsRequest.BasketId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while initializing 3D Secure for BasketId: {BasketId}", threeDsRequest.BasketId);
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
                    Status = PaymentStatusExtension.GetPaymentStatus(payment.Status),
                    ErrorMessage = payment.ErrorMessage,
                    BasketId = payment.BasketId
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
