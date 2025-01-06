namespace Udemy.Common.Events.Payment.Enrollment;

public record Payment3DHtmlGeneratedEvent(Guid UserId, string HtmlContent, string PaymentId, string BasketId);