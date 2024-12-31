using Udemy.Common.Base;

namespace Udemy.Course.Domain.Entities;

public class Favorite : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid CourseId { get; set; }
    public DateTimeOffset AddedAt { get; set; }
}