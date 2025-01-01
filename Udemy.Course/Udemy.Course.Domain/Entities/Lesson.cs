using Udemy.Common.Base;

namespace Udemy.Course.Domain.Entities;

public class Lesson : BaseEntity
{
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string VideoUrl { get; set; } = "";
    public Guid CourseId { get; set; }
    public Guid LessonCategoryId { get; set; }

    public virtual List<Attachment> Attachments { get; set; } = [];
    public virtual List<Question> Questions { get; set; } = [];
    public virtual LessonCategory? LessonCategory { get; set; }
    public virtual Course? Course { get; set; }
}