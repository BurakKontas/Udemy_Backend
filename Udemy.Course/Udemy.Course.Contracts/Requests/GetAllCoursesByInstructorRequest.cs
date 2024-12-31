namespace Udemy.Course.Contracts.Requests;

public class GetAllCoursesByInstructorRequest
{
    public Guid InstructorId { get; set; }
}