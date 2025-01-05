namespace Udemy.Common.Events.Refund;

public record CourseRefundIssuedEvent(Guid UserId, Guid CourseId);