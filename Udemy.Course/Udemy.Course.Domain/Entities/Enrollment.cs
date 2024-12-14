namespace Udemy.Course.Domain.Entities;

public class Enrollment : BaseEntity
{
    public Guid CourseId { get; set; }
    public Guid StudentId { get; set; }
    public DateTime EnrolledAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public List<Guid> CompletedLessons { get; set; } = new();
}