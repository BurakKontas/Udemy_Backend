using Udemy.Common.Base;

namespace Udemy.Course.Domain.Entities;

public class Attachment : BaseEntity
{
    public string Name { get; set; } = "";
    public string Url { get; set; } = "";
    public string Type { get; set; } = "";
    public long Size { get; set; }
    public Guid LessonId { get; set; }
}