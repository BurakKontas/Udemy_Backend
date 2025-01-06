using Iyzipay.Model;
using MassTransit;
using Udemy.Common.Events;
using Udemy.Common.Events.Payment.Enrollment;
using Udemy.Payment.Domain.Dtos;
using Udemy.Payment.Domain.Entities;
using Udemy.Payment.Domain.Enums;
using Udemy.Payment.Domain.Interfaces;
using Udemy.Payment.Domain.Utils;

namespace Udemy.Payment.Application.Handlers;

public class EnrollmentCreatedWithThreeDEventHandler(
    IIyzipayRepository iyzipayRepository,
    IUserDataRepository userDataRepository,
    IPaymentRepository paymentRepository,
    ICardRepository cardRepository
)
    : IConsumer<EnrollmentCreatedWithThreeDEvent>
{
    private readonly IIyzipayRepository _iyzipayRepository = iyzipayRepository;
    private readonly IUserDataRepository _userDataRepository = userDataRepository;
    private readonly IPaymentRepository _paymentRepository = paymentRepository;
    private readonly ICardRepository _cardRepository = cardRepository;

    public async Task Consume(ConsumeContext<EnrollmentCreatedWithThreeDEvent> context)
    {
        var message = context.Message;
        UserDataEntity userData = UserDataEntity.ToEntity(message.UserData);

        var price = message.Enrollments.Sum(x => x.Course.Price);

        var basketItems = message.Enrollments.Select(x =>
            IyzipayUtils.CreateBasketItem(x.Course.Id.ToString(), x.Course.Name, x.Course.Price, IyzipayCategory.Course, BasketItemType.VIRTUAL))
        .ToList();

        var ifCardInformationExists = message.CardInformation is not null;
        var ifCardIdExists = message.CardId is not null;

        if (ifCardInformationExists && ifCardIdExists)
        {
            await context.RespondAsync(new PaymentFailed("Payment failed: Card information and card id cannot be sent together."));
            return;
        }


        var threeDsRequest = new ThreeDSecureRequest()
        {
            Amount = price,
            BasketItems = basketItems,
            BillingAddress = IyzipayUtils.GenerateNaAddress(),
            ShippingAddress = IyzipayUtils.GenerateNaAddress(),
            Currency = "TRY",
            Buyer = IyzipayUtils.CreateBuyer(userData),
            CallbackUrl = message.CallbackUrl,
        };

        if (ifCardIdExists)
        {
            var card = await _cardRepository.GetByIdAsync(message.CardId);
            if (card is null)
            {
                await context.RespondAsync(new PaymentFailed($"Payment failed: Card with {message.CardId} is not exists."));
                return;
            }

            threeDsRequest.PaymentCard = CardEntity.MapToPaymentCard(card);
        }

        if (ifCardInformationExists)
        {
            threeDsRequest.PaymentCard = IyzipayUtils.CreatePaymentCard(message.CardInformation);
        }

        var threeDsPayment = await _iyzipayRepository.InitializeThreeDSecureAsync(threeDsRequest);

        if (threeDsPayment.Status == PaymentStatus.Failure)
        {
            await context.RespondAsync(new PaymentFailed("Payment failed: 3D secure payment failed."));
            return;
        }

        var basket = new BasketEntity
        {
            Id = threeDsPayment.BasketId,
            Items = basketItems.Select(x => BasketItemEntity.MapToBasketItemEntity(x, threeDsPayment.BasketId)).ToList(),
            TotalAmount = price
        };

        var paymentEntity = new PaymentEntity
        {
            Id = threeDsPayment.PaymentId,
            PaymentStatus = threeDsPayment.Status,
            PaymentMethod = PaymentMethod.CreditCard,
            PaymentDate = DateTime.UtcNow,
            Basket = basket,
            UserData = userData,
            TotalAmount = price,
            UserDataId = userData.Id,
            BasketId = threeDsPayment.BasketId,
        };

        await _paymentRepository.AddAsync(paymentEntity);

        await context.RespondAsync(new Payment3DHtmlGeneratedEvent(userData.Id, threeDsPayment.HtmlContent, threeDsPayment.PaymentId, threeDsPayment.BasketId));
    }
}