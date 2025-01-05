namespace Udemy.Payment.Domain.Enums;

public enum PaymentMethod
{
    CreditCard = 1,  // Kredi Kartı
    DebitCard = 2,   // Banka Kartı
    Paypal = 3,      // PayPal
    BankTransfer = 4,// Banka Havalesi
    MobilePayment = 5// Mobil Ödeme (örneğin, mobil cüzdanlar)
}