using Udemy.Common.Base;

namespace Udemy.Course.Domain.Entities;

public class Question : BaseEntity
{
    public Guid UserId { get; set; }
    public string Value { get; set; } = "";
    public int AnswerCount { get; set; }
    public int LikeCount { get; set; }
    public Guid LessonId { get; set; }

    public virtual List<Answer> Answers { get; set; } = [];
    public virtual List<Like> Likes { get; set; } = [];
}