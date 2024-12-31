using System.Collections.Immutable;
using Udemy.Course.Domain.Entities;

namespace Udemy.Course.Domain.Dtos;

public record CourseElasticDto(Guid Id, string Title, string Description, ImmutableArray<string> Tags);