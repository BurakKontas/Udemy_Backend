namespace Udemy.Common.Events.Enrollment;

public record Payment3DHtmlGeneratedEvent(Guid UserId, string HtmlContent, string PaymentId, string BasketId);