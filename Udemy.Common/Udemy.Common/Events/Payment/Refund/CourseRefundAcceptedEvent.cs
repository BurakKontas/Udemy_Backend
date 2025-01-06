namespace Udemy.Common.Events.Payment.Refund;

public record CourseRefundedEvent(Guid UserId, Guid CourseId, decimal Price);