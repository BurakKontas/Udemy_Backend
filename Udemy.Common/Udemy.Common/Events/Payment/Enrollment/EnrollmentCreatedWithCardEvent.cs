using Udemy.Common.Events.Payment.EventDtos;

namespace Udemy.Common.Events.Payment.Enrollment;

public record EnrollmentCreatedWithCardEvent(Guid UserId, ICollection<EnrollmentEventDto> Enrollments, CardInformationEventDto CardInformation, UserDataEventDto? UserData, bool SaveCard = false, string Email = "");