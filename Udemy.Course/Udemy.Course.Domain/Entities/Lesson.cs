namespace Udemy.Course.Domain.Entities;

public class Lesson : BaseEntity
{
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string VideoUrl { get; set; } = "";
    public List<Attachment> Attachments { get; set; } = new();
    public List<Question> Questions { get; set; } = new();
}