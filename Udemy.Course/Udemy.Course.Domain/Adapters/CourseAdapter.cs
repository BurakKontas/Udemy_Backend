using System.Collections.Immutable;
using Udemy.Course.Domain.Dtos;

namespace Udemy.Course.Domain.Adapters;

public static class CourseAdapter
{
    public static CourseElasticDto ToCourseElasticDto(this Entities.Course course)
    {
        var tags = course.Tags.Select(t => t.Name).ToImmutableArray();

        return new CourseElasticDto(course.Id, course.Title, course.CourseDetails?.Description ?? string.Empty, tags);
    }
}