using Udemy.Common.Base;

namespace Udemy.Course.Domain.Entities;

public class Enrollment : BaseEntity
{
    public Guid StudentId { get; set; }
    public DateTimeOffset EnrolledAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? CompletedAt { get; set; } = null;
    public List<Guid> CompletedLessons { get; set; } = [];
    public Guid CourseId { get; set; }

    public Enrollment() { }

    private Enrollment(Guid studentId, Guid courseId)
    {
        StudentId = studentId;
        CourseId = courseId;
    }

    public static Enrollment Create(Guid studentId, Guid courseId)
    {
        return new Enrollment(studentId, courseId);
    }
}