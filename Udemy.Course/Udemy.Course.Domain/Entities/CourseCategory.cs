namespace Udemy.Course.Domain.Entities;

public class CourseCategory : BaseEntity
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public List<Lesson> Lessons { get; set; } = [];
}