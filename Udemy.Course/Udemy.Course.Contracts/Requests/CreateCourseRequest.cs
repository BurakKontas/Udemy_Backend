namespace Udemy.Course.Contracts.Requests;

public class CreateCourseRequest
{
    public Guid InstructorId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string Level { get; set; }
    public string Language { get; set; }
    public bool IsActive { get; set; }
}