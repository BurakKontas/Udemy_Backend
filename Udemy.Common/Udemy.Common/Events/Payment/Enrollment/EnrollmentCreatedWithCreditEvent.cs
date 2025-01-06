using Udemy.Common.Events.Payment.EventDtos;

namespace Udemy.Common.Events.Payment.Enrollment;

public record EnrollmentCreatedWithCreditEvent(Guid UserId, ICollection<EnrollmentEventDto> Enrollments);