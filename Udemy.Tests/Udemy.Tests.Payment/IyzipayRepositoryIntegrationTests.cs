using Iyzipay.Model;
using Microsoft.Extensions.Logging;
using Udemy.Payment.Domain.Dtos;
using Udemy.Payment.Domain.Enums;
using Udemy.Payment.Domain.Utils;
using Udemy.Payment.Infrastructure.Repositories;
using Udemy.Tests.Common;

namespace Udemy.Tests.Payment;

[TestCaseOrderer("Udemy.Tests.Common.PriorityOrderer", "Udemy.Tests.Payment")]
public class IyzipayRepositoryIntegrationTests
{
    private readonly IyzipayRepository _repository;
    private readonly ILogger<IyzipayRepository> _logger;

    public IyzipayRepositoryIntegrationTests()
    {
        var options = new Iyzipay.Options
        {
            ApiKey = "sandbox-pQbUefko7FQR9z2IzdVwLOlsgRc7W2Ks",
            SecretKey = "sandbox-6DQ81qA6A7Yr1iLBsaTMDrrNplzV2qBZ",
            BaseUrl = "https://sandbox-api.iyzipay.com"
        };

        _logger = LoggerFactory.Create(builder => builder.AddConsole())
            .CreateLogger<IyzipayRepository>();

        _repository = new IyzipayRepository(options, _logger);
    }

    [Fact, Priority(1)]
    public async Task CreatePaymentAsync_ShouldCreatePayment_WhenValidRequestIsProvided()
    {
        // Arrange
        var buyer = IyzipayUtils.CreateBuyer(
            id: Guid.NewGuid(),
            name: "Test User",
            surname: "Test",
            email: "test@example.com",
            gsmNumber: "N/A",
            identityNumber: "11111111111",
            ip: "85.34.78.112",
            city: "Istanbul",
            country: "Turkey",
            address: "Test Address",
            zipCode: "N/A",
            registrationDate: DateTime.UtcNow.AddYears(-1),
            lastLoginDate: DateTime.UtcNow
        );

        var billingAddress = IyzipayUtils.CreateAddress(
            contactName: "Test User",
            city: "Istanbul",
            country: "Turkey",
            description: "Test Billing Address",
            zipCode: "N/A"
        );

        var shippingAddress = IyzipayUtils.CreateAddress(
            contactName: "Test User",
            city: "Istanbul",
            country: "Turkey",
            description: "Test Shipping Address",
            zipCode: "N/A"
        );

        var basketItems = IyzipayUtils.CreateBasket(new[]
        {
            (itemId: Guid.NewGuid().ToString(), name: "Test Item", price: 100.50m,
                category: new IyzipayCategory("Physical","N/A" ),
                itemType: BasketItemType.PHYSICAL)
        });

        var paymentCard = IyzipayUtils.CreatePaymentCard(
            cardHolderName: "John Doe",
            cardNumber: "5890040000000016",
            expireMonth: "12",
            expireYear: "2025",
            cvc: "123",
            registerCard: true
        );

        var request = new PaymentRequest()
        {
            PaymentCard = paymentCard,
            BillingAddress = billingAddress,
            ShippingAddress = shippingAddress,
            Currency = "TRY",
            Price = 100.50m,
            BasketItems = basketItems,
            Buyer = buyer,
            Installment = 1
        };

        // Act
        var response = await _repository.CreatePaymentAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.Equal("success", response.Status.ToString().ToLower());
        Assert.False(string.IsNullOrEmpty(response.PaymentId));
    }

    [Fact, Priority(2)]
    public async Task GetPaymentDetailsAsync_ShouldReturnPaymentDetails_WhenPaymentExists()
    {
        // Arrange
        var paymentId = "23352193";

        // Act
        var response = await _repository.GetPaymentDetailsAsync(paymentId);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(paymentId, response.PaymentId);
    }

    [Fact, Priority(3)]
    public async Task RefundPaymentAsync_ShouldRefundPayment_WhenValidRequestIsProvided()
    {
        // Arrange
        var request = new RefundRequest
        {
            PaymentTransactionId = "25330483",
            RefundAmount = 0.6m,
            ConversationId = "123456789",
            Reason = RefundReason.BUYER_REQUEST
        };

        // Act
        var response = await _repository.RefundPaymentAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.Equal("success", response.Status.ToLower());
        Assert.False(string.IsNullOrEmpty(response.RefundId));
    }
}