using Udemy.Common.Base;

namespace Udemy.Course.Domain.Entities;

public class Answer : BaseEntity
{
    public string Value { get; set; } = "";
    public Guid UserId { get; set; }
    public Guid QuestionId { get; set; }
}