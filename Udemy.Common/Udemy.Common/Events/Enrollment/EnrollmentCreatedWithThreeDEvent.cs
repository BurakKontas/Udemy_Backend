using Udemy.Common.Events.Dtos;

namespace Udemy.Common.Events.Enrollment;

public record EnrollmentCreatedWithThreeDEvent(Guid UserId, ICollection<(CourseEventDto course, Guid enrollmentId)> Enrollments);