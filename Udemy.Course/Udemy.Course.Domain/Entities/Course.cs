using Udemy.Common.Base;
using Udemy.Course.Domain.Enums;

namespace Udemy.Course.Domain.Entities;

public class Course : BaseEntity, ICloneable
{
    public List<Guid> InstructorIds { get; set; } = [];
    public string Title { get; set; } = "";
    public string Language { get; set; }
    public CourseLevel Level { get; set; }
    public bool IsActive { get; set; }
    public bool IsApproved { get; set; }
    public DateTimeOffset? ApprovedAt { get; set; }
    public bool IsFeatured { get; set; }
    public bool HasCertificate { get; set; }
    public string? CertificateTemplateUrl { get; set; }

    public virtual ICollection<AuditLog> AuditLogs { get; set; } = [];
    public virtual ICollection<Comment> Comments { get; set; } = [];
    public virtual ICollection<Favorite> Favorites { get; set; } = [];
    public virtual ICollection<Tag> Tags { get; set; } = [];
    public virtual ICollection<LessonCategory> LessonCategories { get; set; } = [];
    public virtual ICollection<Lesson> Lessons { get; set; } = [];
    public virtual ICollection<Enrollment> Enrollments { get; set; } = [];

    public virtual CourseDetails? CourseDetails { get; set; }

    // EF Core Constructor
    public Course() { }

    private Course(
        string title,
        CourseLevel level,
        string language,
        bool isActive)
    {
        Title = title;
        Level = level;
        Language = language;
        IsActive = isActive;
    }

    // Static Factory Method
    public static Course Create(
        Guid instructorId,
        string title,
        CourseLevel level,
        string language = "English",
        bool isActive = true)
    {
        var course = new Course(title, level, language, isActive);
        course.AssignInstructor(instructorId);
        return course;
    }

    public void AssignInstructor(Guid instructorId)
    {
        if (InstructorIds.Contains(instructorId))
            return;

        InstructorIds.Add(instructorId);
    }


    public void UpdateStatus(bool isActive)
    {
        IsActive = isActive;
    }

    public void UpdateApprovalStatus(bool isApproved)
    {
        IsApproved = isApproved;
        ApprovedAt = isApproved ? DateTimeOffset.UtcNow : null;
    }

    public void SetDetails(CourseDetails courseDetails)
    {
        CourseDetails = courseDetails;
    }

    public object Clone()
    {
        return new Course
        {
            Id = this.Id,
            InstructorIds = [..this.InstructorIds],
            Title = this.Title,
            Level = this.Level,
            Language = this.Language,
            IsActive = this.IsActive,
            IsApproved = this.IsApproved,
            ApprovedAt = this.ApprovedAt,
            IsFeatured = this.IsFeatured,
            HasCertificate = this.HasCertificate,
            CertificateTemplateUrl = this.CertificateTemplateUrl,
            Lessons = new List<Lesson>(this.Lessons),
            Tags = new List<Tag>(this.Tags),
            Enrollments = new List<Enrollment>(this.Enrollments)
        };
    }
}