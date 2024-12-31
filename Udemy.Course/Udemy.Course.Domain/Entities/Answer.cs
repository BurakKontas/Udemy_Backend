using Udemy.Common.Base;

namespace Udemy.Course.Domain.Entities;

public class Answer : BaseEntity
{
    public Guid QuestionId { get; set; }
    public string Value { get; set; } = "";
    public Guid UserId { get; set; }
}