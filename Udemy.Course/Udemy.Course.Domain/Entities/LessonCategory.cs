using Udemy.Common.Base;

namespace Udemy.Course.Domain.Entities;

public class LessonCategory : BaseEntity
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public Guid CourseId { get; set; }

    public virtual ICollection<Lesson> Lessons { get; set; } = [];
}