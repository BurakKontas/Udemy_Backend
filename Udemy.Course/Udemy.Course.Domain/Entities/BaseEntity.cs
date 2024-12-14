namespace Udemy.Course.Domain.Entities;

// Base Entity for Common Properties
public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

// Enum Definitions