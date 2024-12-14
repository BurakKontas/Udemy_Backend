namespace Udemy.Course.Domain.Entities;

public class CourseDetails : BaseEntity
{
    public Guid CourseId { get; set; }
    public int RateCount { get; set; }
    public int RateAverage { get; set; }
    public List<Rate> Rates { get; set; } = new();
    public List<Comment> Comments { get; set; } = new();
    public int ViewCount { get; set; }
    public TimeSpan TotalDuration { get; set; }
}