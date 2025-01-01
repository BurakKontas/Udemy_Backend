using Udemy.Common.Base;

namespace Udemy.Course.Domain.Entities;

public class AuditLog : BaseEntity
{
    public Guid UserId { get; set; }
    public string Action { get; set; } = "";
    public string Details { get; set; } = "";
    public Guid CourseId { get; set; }

    public virtual Course? Course { get; set; }
}