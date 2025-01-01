using Udemy.Common.Base;

namespace Udemy.Course.Domain.Entities;

public class Comment : BaseEntity
{
    public Guid UserId { get; set; }
    public string Value { get; set; } = "";
    public Guid CourseId { get; set; }

    public virtual Rate? Rate { get; set; }
}