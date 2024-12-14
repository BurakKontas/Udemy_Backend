namespace Udemy.Course.Domain.Entities;

public class Favorite : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid CourseId { get; set; }
    public DateTime AddedAt { get; set; }
}