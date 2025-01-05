﻿using Iyzipay.Model;
using Iyzipay.Request;
using Udemy.Payment.Domain.Enums;

namespace Udemy.Payment.Domain.Utils;

public static class IyzipayUtils
{
    public static Address CreateAddress(string contactName, string city, string country, string description, string zipCode)
    {
        var address = new Address();
        address.ContactName = contactName;
        address.City = city;
        address.Country = country;
        address.Description = description;
        address.ZipCode = zipCode;

        return address;
    }

    public static BasketItem CreateBasketItem(Guid itemId, string name, decimal price, IyzipayCategory category, BasketItemType itemType)
    {
        var item = new BasketItem();
        item.Id = itemId.ToString();
        item.Name = name;
        item.Price = price.ToString("F2");
        item.Category1 = category.Primary;
        item.Category2 = category.Secondary;
        item.ItemType = itemType.ToString();

        return item;
    }

    public static List<BasketItem> CreateBasket(IEnumerable<(Guid itemId, string name, decimal price, IyzipayCategory category, BasketItemType itemType)> items)
    {
        return items.Select(item => CreateBasketItem(item.itemId, item.name, item.price, item.category, item.itemType)).ToList();
    }

    public static Buyer CreateBuyer(
        Guid id,
        string name,
        string surname,
        string email,
        string gsmNumber,
        string identityNumber,
        string ip,
        string city,
        string country,
        string address,
        string zipCode,
        DateTime registrationDate,
        DateTime lastLoginDate)
    {
        var buyer = new Buyer
        {
            Id = id.ToString(),
            Name = name,
            Surname = surname,
            Email = email,
            GsmNumber = gsmNumber,
            IdentityNumber = identityNumber,
            LastLoginDate = lastLoginDate.ToString("yyyy-MM-dd HH:mm:ss"),
            RegistrationDate = registrationDate.ToString("yyyy-MM-dd HH:mm:ss"),
            RegistrationAddress = address,
            Ip = ip,
            City = city,
            Country = country,
            ZipCode = zipCode
        };

        return buyer;
    }


    public static PaymentCard CreatePaymentCard(string cardHolderName, string cardNumber, string expireMonth, string expireYear, string cvc, bool registerCard = false)
    {
        var card = new PaymentCard();
        card.CardHolderName = cardHolderName;
        card.CardNumber = cardNumber;
        card.ExpireMonth = expireMonth;
        card.ExpireYear = expireYear;
        card.Cvc = cvc;
        card.RegisterCard = registerCard ? 1 : 0;

        return card;
    }

    public static CreatePaymentRequest CreatePaymentRequest(
        string conversationId,
        decimal price,
        string basketId,
        PaymentChannel paymentChannel,
        PaymentGroup paymentGroup,
        Buyer buyer,
        Address billingAddress,
        Address shippingAddress,
        List<BasketItem> basketItems,
        PaymentCard paymentCard,
        int installment = 1,
        decimal taxRate = 0.20m, // KDV
        decimal additionalFee = 0.0m 
    )
    {
        var paidPrice = price + (price * taxRate) + additionalFee;

        var request = new CreatePaymentRequest
        {
            Locale = Locale.TR.ToString(),
            ConversationId = conversationId,
            Price = price.ToString("F2"),
            PaidPrice = paidPrice.ToString("F2"),
            Currency = Currency.TRY.ToString(),
            Installment = installment,
            BasketId = basketId,
            PaymentChannel = paymentChannel.ToString(),
            PaymentGroup = paymentGroup.ToString(),
            Buyer = buyer,
            BillingAddress = billingAddress,
            ShippingAddress = shippingAddress,
            BasketItems = basketItems,
            PaymentCard = paymentCard
        };

        return request;
    }

    public static string GenerateId(string prefix = "")
    {
        return $"{prefix}{Ulid.NewUlid()}";
    }
}
