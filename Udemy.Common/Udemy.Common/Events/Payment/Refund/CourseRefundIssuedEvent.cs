namespace Udemy.Common.Events.Payment.Refund;

public record CourseRefundIssuedEvent(Guid UserId, Guid CourseId);