using Udemy.Course.Domain.Enums;

namespace Udemy.Course.Domain.Entities;

public class Course : BaseEntity, ICloneable
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
    public CourseLevel Level { get; set; }
    public string Language { get; set; }
    public bool IsActive { get; set; }
    public bool IsApproved { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public bool IsFeatured { get; set; }
    public string? PreviewVideoUrl { get; set; }
    public bool HasCertificate { get; set; }
    public string? CertificateTemplateUrl { get; set; }
    public int RateCount { get; set; }
    public decimal RateValue { get; set; }
    public decimal AverageRate => RateCount == 0 ? 0 : Math.Round(RateValue / RateCount, 2);

    public virtual ICollection<Enrollment> Enrollments { get; set; } = [];

    // EF Core Constructor
    protected Course() { }

    private Course(
        string title,
        string description,
        decimal price,
        CourseLevel level,
        string language,
        bool isActive)
    {
        Title = title;
        Description = description;
        Price = price;
        Level = level;
        Language = language;
        IsActive = isActive;
    }

    // Static Factory Method
    public static Course Create(
        Guid instructorId,
        string title,
        string description,
        decimal price,
        CourseLevel level,
        string language = "English",
        bool isActive = true)
    {
        var course = new Course(title, description, price, level, language, isActive);
        course.AssignInstructor(instructorId);
        return course;
    }

    public void AssignInstructor(Guid instructorId)
    {
        if (InstructorIds.Contains(instructorId))
            return;

        InstructorIds.Add(instructorId);
    }

    public void AddRating(int ratingValue)
    {
        RateCount++;
        RateValue += ratingValue;
    }

    public void RemoveRating(int ratingValue)
    {
        RateCount--;
        RateValue -= ratingValue;
    }

    public void UpdateDiscount(decimal discountedPrice, DateTime startDate, DateTime endDate)
    {
        DiscountedPrice = discountedPrice;
        DiscountStartDate = startDate;
        DiscountEndDate = endDate;
    }

    public void UpdateDiscountEndDate(DateTime endDate)
    {
        DiscountEndDate = endDate;
    }

    public void UpdateDiscountStartDate(DateTime startDate)
    {
        DiscountStartDate = startDate;
    }

    public void UpdateStatus(bool isActive)
    {
        IsActive = isActive;
    }

    public object Clone()
    {
        return new Course
        {
            Id = this.Id,
            InstructorIds = new List<Guid>(this.InstructorIds),
            Title = this.Title,
            Description = this.Description,
            ImageUrl = this.ImageUrl,
            Categories = new List<LessonCategory>(this.Categories),
            Lessons = new List<Lesson>(this.Lessons),
            Tags = new List<Tag>(this.Tags),
            Price = this.Price,
            DiscountedPrice = this.DiscountedPrice,
            DiscountStartDate = this.DiscountStartDate,
            DiscountEndDate = this.DiscountEndDate,
            Level = this.Level,
            Language = this.Language,
            IsActive = this.IsActive,
            IsApproved = this.IsApproved,
            ApprovedAt = this.ApprovedAt,
            IsFeatured = this.IsFeatured,
            PreviewVideoUrl = this.PreviewVideoUrl,
            HasCertificate = this.HasCertificate,
            CertificateTemplateUrl = this.CertificateTemplateUrl,
            RateCount = this.RateCount,
            RateValue = this.RateValue,
            Enrollments = new List<Enrollment>(this.Enrollments)
        };
    }
}