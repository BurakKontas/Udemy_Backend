using Udemy.Common.Events.Dtos;
using Udemy.Common.Events.EventDtos;

namespace Udemy.Common.Events.Enrollment;

public record EnrollmentPaidEvent(Guid UserId, ICollection<EnrollmentEventDto> Enrollments);