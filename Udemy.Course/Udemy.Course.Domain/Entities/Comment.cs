namespace Udemy.Course.Domain.Entities;

public class Comment : BaseEntity
{
    public Guid UserId { get; set; }
    public string Text { get; set; } = "";
    public Rate Rate { get; set; } = new();
}