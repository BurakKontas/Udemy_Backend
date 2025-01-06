namespace Udemy.Common.Events.Payment.EventDtos;

public record EnrollmentEventDto(CourseEventDto Course, Guid EnrollmentId);