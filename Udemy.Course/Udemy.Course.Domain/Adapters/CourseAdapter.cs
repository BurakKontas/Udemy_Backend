using Udemy.Course.Domain.Dtos;

namespace Udemy.Course.Domain.Adapters;

public static class CourseAdapter
{
    public static CourseElasticDto ToCourseElasticDto(this Entities.Course course)
    {
        return new CourseElasticDto(course.Id, course.Title, course.Description, course.Tags);
    }
}