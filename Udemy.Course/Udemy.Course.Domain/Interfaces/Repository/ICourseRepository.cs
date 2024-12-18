using Udemy.Common.Base;

namespace Udemy.Course.Domain.Interfaces.Repository;

public interface ICourseRepository : IBaseRepository<Entities.Course>
{
    public Task<IEnumerable<Entities.Course>> GetCoursesByInstructorAsync(Guid instructorId);
    public Task<IEnumerable<Entities.Course>> GetFeaturedCoursesAsync();
    public Task<IEnumerable<Entities.Course>> GetCoursesByKeywordAsync(string keyword);
}