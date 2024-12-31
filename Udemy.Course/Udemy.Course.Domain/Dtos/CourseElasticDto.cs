using Udemy.Course.Domain.Entities;

namespace Udemy.Course.Domain.Dtos;

public record CourseElasticDto(Guid Id, string Title, string Description, List<Tag> Tags);