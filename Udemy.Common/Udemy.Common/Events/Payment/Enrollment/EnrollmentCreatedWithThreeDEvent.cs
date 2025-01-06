using Udemy.Common.Events.Payment.EventDtos;

namespace Udemy.Common.Events.Payment.Enrollment;

public record EnrollmentCreatedWithThreeDEvent(Guid UserId, ICollection<EnrollmentEventDto> Enrollments, UserDataEventDto UserData, string CallbackUrl, CardInformationEventDto? CardInformation, string? CardId);