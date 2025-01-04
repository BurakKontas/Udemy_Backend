using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Enums;

namespace Udemy.Course.Domain.Interfaces.Service;

public interface ICourseService
{
    Task<Guid> CreateAsync(Guid instructorId, string title, string description, decimal price, CourseLevel level, string language, bool isActive);
    Task<Guid> UpdateAsync(Guid userId, Guid courseId, Dictionary<string, object> updates);
    Task<Guid> DeleteAsync(Guid userId, Guid courseId);
    Task<Entities.Course?> GetByIdAsync(Guid courseId);
    Task<IEnumerable<Entities.Course>> GetAllByInstructorAsync(Guid instructorId, EndpointFilter filter);
    Task<IEnumerable<Entities.Course>> SearchCoursesAsync(string keyword, EndpointFilter filter);
    Task<IEnumerable<Entities.Course>> GetFeaturedCoursesAsync(EndpointFilter filter);
    Task<Guid> AssignInstructorAsync(Guid courseId, Guid instructorId);
    Task<Guid> UpdateStatusAsync(Guid userId, Guid courseId, bool isActive);
    Task<Entities.Course[]> GetAllCourses(EndpointFilter filter); // will be only used for debug purposes
}