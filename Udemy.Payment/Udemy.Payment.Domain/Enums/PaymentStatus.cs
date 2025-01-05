namespace Udemy.Payment.Domain.Enums;

public enum PaymentStatus
{
    Pending = 1,        // Beklemede (Ödeme işlemi hala işleniyor)
    Successful = 2,     // Başarılı (Ödeme başarıyla tamamlandı)
    Failed = 3,         // Başarısız (Ödeme başarısız oldu)
    Refunded = 4,       // İade Edildi (Ödeme geri ödendi)
    Canceled = 5,       // İptal Edildi (Ödeme iptal edildi)
    UnderReview = 6,    // İnceleniyor (Ödeme incelemeye alındı)
    ChargedBack = 7     // Ters İade (Kredi kartı şirketi ödeme işlemini geri aldı)
}

