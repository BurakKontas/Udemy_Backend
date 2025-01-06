using Udemy.Common.Events.Payment.EventDtos;

namespace Udemy.Common.Events.Payment.Enrollment;

public record EnrollmentCreatedWithStoredCardEvent(Guid UserId, ICollection<EnrollmentEventDto> Enrollments, string CardId, UserDataEventDto? UserData);