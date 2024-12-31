using Udemy.Common.Base;

namespace Udemy.Course.Domain.Entities;

public class Enrollment : BaseEntity
{
    public Guid CourseId { get; set; }
    public Guid StudentId { get; set; }
    public DateTimeOffset EnrolledAt { get; set; }
    public DateTimeOffset? CompletedAt { get; set; }
    public List<Guid> CompletedLessons { get; set; } = new();
}