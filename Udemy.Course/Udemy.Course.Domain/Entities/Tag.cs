using Udemy.Common.Base;

namespace Udemy.Course.Domain.Entities;

public class Tag : BaseEntity
{
    public string Name { get; set; } = "";
    public Guid CourseId { get; set; }

    public virtual Course? Course { get; set; }
}