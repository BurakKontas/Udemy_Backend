using Udemy.Common.Base;

namespace Udemy.Course.Domain.Entities;

public class Like : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid QuestionId { get; set; }
    public Guid AnswerId { get; set; }
}