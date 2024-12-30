using Udemy.Common.Base;
using Udemy.Common.ModelBinder;

namespace Udemy.Course.Domain.Interfaces.Repository;

public interface ICourseRepository : IBaseRepository<Entities.Course>
{
    public Task<IEnumerable<Entities.Course>> GetCoursesByInstructorAsync(Guid instructorId, EndpointFilter filter);
    public Task<IEnumerable<Entities.Course>> GetFeaturedCoursesAsync(EndpointFilter filter);
    public Task<IEnumerable<Entities.Course>> GetCoursesByKeywordAsync(string keyword, EndpointFilter filter);
}