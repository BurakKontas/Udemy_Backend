using Udemy.Common.Events.EventDtos;

namespace Udemy.Common.Events.Enrollment;

public record EnrollmentCreatedWithCardEvent(Guid UserId, ICollection<EnrollmentEventDto> Enrollments, CardInformationEventDto CardInformation, bool SaveCard = false);