namespace Udemy.Course.Domain.Entities;

public class CourseAggregate : BaseEntity
{
    public Course Course { get; set; } = new();
    public CourseDetails Details { get; set; } = new();
    public List<Lesson> Lessons { get; set; } = [];
    public List<Enrollment> Enrollments { get; set; } = [];
    public List<AuditLog> AuditLogs { get; set; } = [];
    public List<Comment> Comments { get; set; } = [];
    public List<Rate> Rates { get; set; } = [];
    public List<Tag> Tags { get; set; } = [];

    public decimal GetEffectivePrice()
    {
        if (Course.DiscountedPrice.HasValue &&
            DateTime.Now >= Course.DiscountStartDate &&
            DateTime.Now <= Course.DiscountEndDate)
        {
            return Course.DiscountedPrice.Value;
        }
        return Course.Price;
    }

    public void AddLesson(Lesson lesson)
    {
        Lessons.Add(lesson);
    }

    public void EnrollStudent(Guid studentId)
    {
        Enrollments.Add(new Enrollment
        {
            CourseId = Course.Id,
            StudentId = studentId,
            EnrolledAt = DateTime.Now
        });
    }
}