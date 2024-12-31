using Udemy.Common.Base;

namespace Udemy.Course.Domain.Entities;

public class Question : BaseEntity
{
    public Guid UserId { get; set; }
    public string Value { get; set; } = "";
    public List<Answer> Answers { get; set; } = new();
    public int AnswerCount { get; set; }
    public int LikeCount { get; set; }
    public List<Like> Likes { get; set; } = new();
}