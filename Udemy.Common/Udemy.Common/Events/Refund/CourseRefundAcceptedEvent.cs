namespace Udemy.Common.Events.Refund;

public record CourseRefundedEvent(Guid UserId, Guid CourseId, decimal Price);