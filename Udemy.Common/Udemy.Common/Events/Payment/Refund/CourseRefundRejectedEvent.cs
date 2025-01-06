namespace Udemy.Common.Events.Payment.Refund;

public record CourseRefundRejectedEvent(Guid UserId, Guid CourseId, string Reason);