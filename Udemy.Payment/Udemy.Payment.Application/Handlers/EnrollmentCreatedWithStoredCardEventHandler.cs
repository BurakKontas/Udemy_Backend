using Iyzipay.Model;
using MassTransit;
using Udemy.Common.Events;
using Udemy.Common.Events.Enrollment;
using Udemy.Payment.Domain.Dtos;
using Udemy.Payment.Domain.Entities;
using Udemy.Payment.Domain.Enums;
using Udemy.Payment.Domain.Interfaces;
using Udemy.Payment.Domain.Utils;

namespace Udemy.Payment.Application.Handlers;

public class EnrollmentCreatedWithStoredCardEventHandler(
    IIyzipayRepository iyzipayRepository,
    IUserDataRepository userDataRepository,
    IPaymentRepository paymentRepository,
    ICardRepository cardRepository
    )
    : IConsumer<EnrollmentCreatedWithStoredCardEvent>
{
    private readonly IIyzipayRepository _iyzipayRepository = iyzipayRepository;
    private readonly IUserDataRepository _userDataRepository = userDataRepository;
    private readonly IPaymentRepository _paymentRepository = paymentRepository;
    private readonly ICardRepository _cardRepository = cardRepository;

    public async Task Consume(ConsumeContext<EnrollmentCreatedWithStoredCardEvent> context)
    {
        var message = context.Message;
        UserDataEntity userData;

        if (message.UserData is null)
        {
            userData = await _userDataRepository.GetByIdAsync(message.UserId);
            if (userData is null)
            {
                await context.RespondAsync(new UserDataRequiredEvent(message.UserId));
                return;
            }
        }
        else
        {
            userData = UserDataEntity.ToEntity(message.UserData);
        }

        var card = await _cardRepository.GetByIdAsync(message.CardId);

        if (card is null)
        {
            await context.RespondAsync(new PaymentFailed($"Payment failed: Card with {message.CardId} is not exists."));
            return;
        }

        var price = message.Enrollments.Sum(x => x.Course.Price);

        var basketItems = message.Enrollments.Select(x =>
            IyzipayUtils.CreateBasketItem(x.Course.Id, x.Course.Name, x.Course.Price, IyzipayCategory.Course, BasketItemType.VIRTUAL))
        .ToList();

        var storedCardPaymentRequest = new StoredCardPaymentRequest
        {
            Amount = price,
            Currency = "TRY",
            Installment = 1,
            BasketItems = basketItems,
            BillingAddress = IyzipayUtils.GenerateNaAddress(),
            ShippingAddress = IyzipayUtils.GenerateNaAddress(),
            Buyer = IyzipayUtils.CreateBuyer(userData),
            CardToken = card.CardToken,
            UserKey = card.CardUserKey
        };

        var payment = await _iyzipayRepository.CreatePaymentWithStoredCardAsync(storedCardPaymentRequest);

        if (payment.Status == PaymentStatus.Successful)
        {
            var basket = new BasketEntity
            {
                Id = payment.BasketId,
                Items = basketItems.Select(x => BasketItemEntity.MapToBasketItemEntity(x, payment.BasketId)).ToList(),
                TotalAmount = price
            };

            var paymentEntity = new PaymentEntity
            {
                Id = payment.PaymentId,
                Card = card,
                PaymentStatus = payment.Status,
                PaymentMethod = PaymentMethod.CreditCard,
                PaymentDate = DateTime.UtcNow,
                Basket = basket,
                UserData = userData,
                TotalAmount = price
            };

            await _paymentRepository.AddAsync(paymentEntity);

            await context.RespondAsync(new EnrollmentPaidEvent(userData.Id, payment.PaymentId, message.Enrollments));
        }
        else
        {
            await context.RespondAsync(new PaymentFailed($"Payment failed: {payment.ErrorMessage}"));
        }
    }
}