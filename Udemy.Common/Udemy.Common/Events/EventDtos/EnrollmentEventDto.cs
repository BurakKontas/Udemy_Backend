using Udemy.Common.Events.Dtos;

namespace Udemy.Common.Events.EventDtos;

public record EnrollmentEventDto(CourseEventDto Course, Guid EnrollmentId);