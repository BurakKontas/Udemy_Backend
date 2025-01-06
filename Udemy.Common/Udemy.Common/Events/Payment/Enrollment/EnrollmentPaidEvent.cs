using Udemy.Common.Events.Payment.EventDtos;

namespace Udemy.Common.Events.Payment.Enrollment;

public record EnrollmentPaidEvent(Guid UserId, string PaymentId, ICollection<EnrollmentEventDto> Enrollments);