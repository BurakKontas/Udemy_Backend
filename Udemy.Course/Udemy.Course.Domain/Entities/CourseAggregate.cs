namespace Udemy.Course.Domain.Entities;

public class CourseAggregate : BaseEntity
{
    public Course Course { get; set; }
    public CourseDetails Details { get; set; } = new();
    public List<LessonCategory> Categories { get; set; } = [];
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

    public void AddLesson(LessonCategory category, Lesson lesson)
    {
        if (!Categories.Contains(category))
        {
            Categories.Add(category);
        }
        Lessons.Add(lesson);
        category.Lessons.Add(lesson);
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

    public void CompleteLesson(Guid studentId, Guid lessonId)
    {
        var enrollment = Enrollments.FirstOrDefault(x => x.StudentId == studentId);
        if (enrollment != null)
        {
            if (!enrollment.CompletedLessons.Contains(lessonId))
            {
                enrollment.CompletedLessons.Add(lessonId);
            }
            if (enrollment.CompletedLessons.Count == Lessons.Count)
            {
                enrollment.CompletedAt = DateTime.Now;
            }
        }
    }

    public void AddComment(Comment comment)
    {
        Comments.Add(comment);
    }

    public void AddRate(Rate rate)
    {
        Rates.Add(rate);
        Details.RateCount++;
        Details.RateAverage = (Details.RateAverage + rate.Value) / Details.RateCount;
    }

    public void AddTag(Tag tag)
    {
        Tags.Add(tag);
    }

    public void AddAuditLog(AuditLog auditLog)
    {
        AuditLogs.Add(auditLog);
    }

    public void RemoveAuditLog(Guid auditLogId)
    {
        var auditLog = AuditLogs.FirstOrDefault(x => x.Id == auditLogId);
        if (auditLog != null)
        {
            AuditLogs.Remove(auditLog);
        }
    }

    public void RemoveComment(Guid commentId)
    {
        var comment = Comments.FirstOrDefault(x => x.Id == commentId);
        if (comment != null)
        {
            Comments.Remove(comment);
        }
    }

    public void RemoveRate(Guid rateId)
    {
        var rate = Rates.FirstOrDefault(x => x.Id == rateId);
        if (rate != null)
        {
            Rates.Remove(rate);
        }
    }

    public void RemoveTag(Guid tagId)
    {
        var tag = Tags.FirstOrDefault(x => x.Id == tagId);
        if (tag != null)
        {
            Tags.Remove(tag);
        }
    }

    public void RemoveLesson(Guid lessonId)
    {
        var lesson = Lessons.FirstOrDefault(x => x.Id == lessonId);
        if (lesson != null)
        {
            Lessons.Remove(lesson);
        }
    }

    public void RemoveCategory(Guid categoryId)
    {
        var category = Categories.FirstOrDefault(x => x.Id == categoryId);
        if (category != null)
        {
            Categories.Remove(category);
        }
    }

    public void RemoveEnrollment(Guid studentId)
    {
        var enrollment = Enrollments.FirstOrDefault(x => x.StudentId == studentId);
        if (enrollment != null)
        {
            Enrollments.Remove(enrollment);
        }
    }
}