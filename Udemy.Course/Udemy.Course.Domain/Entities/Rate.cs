using Udemy.Common.Base;

namespace Udemy.Course.Domain.Entities;

public class Rate : BaseEntity
{
    public Guid UserId { get; set; }
    public int Value { get; set; }
}