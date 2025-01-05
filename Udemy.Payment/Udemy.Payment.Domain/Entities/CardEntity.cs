using Iyzipay.Model;
using Udemy.Common.Base;
using IyzipayPayment = Iyzipay.Model.Payment;

namespace Udemy.Payment.Domain.Entities;

public class CardEntity : BaseEntity
{
    public new required string Id { get; set; } = "None";
    public string CardUserKey { get; set; } = "None";
    public string CardToken { get; set; } = "None";
    public string BinNumber { get; set; } = "None";
    public string LastFourDigits { get; set; } = "0000";
    public string CardType { get; set; } = "None";
    public string CardAssociation { get; set; } = "None";
    public string CardFamily { get; set; } = "None";
    public string? CardAlias { get; set; } = "None";
    public long? CardBankCode { get; set; } = 0;
    public string? CardBankName { get; set; } = "None";

    public virtual ICollection<PaymentEntity> Payments { get; set; } = new List<PaymentEntity>();

    public static CardEntity MapToCardEntity(Card card)
    {
        return new CardEntity
        {
            Id = card.ExternalId,
            CardUserKey = card.CardUserKey,
            CardToken = card.CardToken,
            CardAlias = card.CardAlias,
            BinNumber = card.BinNumber,
            LastFourDigits = card.LastFourDigits,
            CardType = card.CardType,
            CardAssociation = card.CardAssociation,
            CardFamily = card.CardFamily,
            CardBankCode = card.CardBankCode,
            CardBankName = card.CardBankName
        };
    }

    public static CardEntity MapToCardEntity(IyzipayPayment payment)
    {
        return new CardEntity
        {
            Id = payment.PaymentId,
            CardUserKey = payment.CardUserKey,
            CardToken = payment.CardToken,
            BinNumber = payment.BinNumber,
            LastFourDigits = payment.LastFourDigits,
            CardType = payment.CardType,
            CardAssociation = payment.CardAssociation,
            CardFamily = payment.CardFamily,
        };
    }

    public static Card MapToCard(CardEntity cardEntity)
    {
        return new Card
        {
            ExternalId = cardEntity.Id,
            CardUserKey = cardEntity.CardUserKey,
            CardToken = cardEntity.CardToken,
            CardAlias = cardEntity.CardAlias,
            BinNumber = cardEntity.BinNumber,
            LastFourDigits = cardEntity.LastFourDigits,
            CardType = cardEntity.CardType,
            CardAssociation = cardEntity.CardAssociation,
            CardFamily = cardEntity.CardFamily,
            CardBankCode = cardEntity.CardBankCode,
            CardBankName = cardEntity.CardBankName
        };
    }

    public static PaymentCard MapToPaymentCard(CardEntity cardEntity)
    {
        return new PaymentCard
        {
            CardUserKey = cardEntity.CardUserKey,
            CardToken = cardEntity.CardToken,
            CardAlias = cardEntity.CardAlias,
        };
    }

}