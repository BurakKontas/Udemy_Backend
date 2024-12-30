using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Enums;

namespace Udemy.Course.Domain.Interfaces.Service;

public interface ICourseService
{
    Task CreateAsync(Guid instructorId, string title, string description, decimal price, CourseLevel level, string language, bool isActive);
    Task UpdateAsync(Guid courseId, Dictionary<string, object> updates);
    Task DeleteAsync(Guid courseId);
    Task<Entities.Course?> GetByIdAsync(Guid courseId);
    Task<IEnumerable<Entities.Course>> GetAllByInstructorAsync(Guid instructorId, EndpointFilter filter);
    Task<IEnumerable<Entities.Course>> SearchCoursesAsync(string keyword, EndpointFilter filter);
    Task<IEnumerable<Entities.Course>> GetFeaturedCoursesAsync(EndpointFilter filter);
    Task AssignInstructorAsync(Guid courseId, Guid instructorId);
    Task UpdateStatusAsync(Guid courseId, bool isActive);
}