using Udemy.Course.Domain.Enums;

namespace Udemy.Course.Domain.Entities;

public class Course : BaseEntity
{
    public List<Guid> InstructorIds { get; set; } = [];
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string? ImageUrl { get; set; }
    public List<LessonCategory> Categories { get; set; } = [];
    public List<Lesson> Lessons { get; set; } = [];
    public List<Tag> Tags { get; set; } = [];
    public decimal Price { get; set; }
    public decimal? DiscountedPrice { get; set; }
    public DateTime? DiscountStartDate { get; set; }
    public DateTime? DiscountEndDate { get; set; }
    public CourseLevel Level { get; set; } = CourseLevel.All;
    public string Language { get; set; } = "English";
    public bool IsActive { get; set; }
    public bool IsApproved { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public bool IsFeatured { get; set; }
    public string? PreviewVideoUrl { get; set; }
    public bool HasCertificate { get; set; }
    public string? CertificateTemplateUrl { get; set; }

    public virtual ICollection<Enrollment> Enrollments { get; set; } = [];
}