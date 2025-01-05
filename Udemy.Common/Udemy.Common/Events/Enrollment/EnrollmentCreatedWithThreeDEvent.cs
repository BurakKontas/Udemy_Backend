using Udemy.Common.Events.Dtos;
using Udemy.Common.Events.EventDtos;

namespace Udemy.Common.Events.Enrollment;

public record EnrollmentCreatedWithThreeDEvent(Guid UserId, ICollection<EnrollmentEventDto> Enrollments, UserDataEventDto UserData, string CallbackUrl, CardInformationEventDto? CardInformation, string? CardId);