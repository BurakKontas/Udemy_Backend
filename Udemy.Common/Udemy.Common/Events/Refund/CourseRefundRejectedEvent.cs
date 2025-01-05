namespace Udemy.Common.Events.Refund;

public record CourseRefundRejectedEvent(Guid UserId, Guid CourseId, string Reason);