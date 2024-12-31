namespace Udemy.Course.Contracts.Requests;

public class CourseAssignInstructorRequest
{
    public Guid InstructorId { get; set; }
    public Guid CourseId { get; set; }
}