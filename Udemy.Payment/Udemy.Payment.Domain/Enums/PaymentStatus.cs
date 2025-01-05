namespace Udemy.Payment.Domain.Enums;

public enum PaymentStatus
{
    Pending,        // Beklemede (Ödeme işlemi hala işleniyor)
    Successful,     // Başarılı (Ödeme başarıyla tamamlandı)
    Failure,         // Başarısız (Ödeme başarısız oldu)
    Refunded,       // İade Edildi (Ödeme geri ödendi)
    Canceled,       // İptal Edildi (Ödeme iptal edildi)
    UnderReview,    // İnceleniyor (Ödeme incelemeye alındı)
    ChargedBack,     // Ters İade (Kredi kartı şirketi ödeme işlemini geri aldı)
    Success
}

