using Microsoft.AspNetCore.Mvc;
using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Enums;
using Udemy.Course.Domain.Interfaces.Service;

namespace Udemy.Course.API.Controllers;

[Route("/")]
[ApiController]
public class CourseController(ICourseService courseService) : ControllerBase
{
    private readonly ICourseService _courseService = courseService;

    // Create a course
    [HttpPost("/create")]
    public IActionResult CreateCourse([FromBody] object request)
    {
        return Ok();
    }

    // Update a course
    [HttpPost("/update")]
    public IActionResult UpdateCourse([FromBody] object request)
    {
        return Ok();
    }

    // Delete a course
    [HttpDelete("/delete/{courseId}")]
    public IActionResult DeleteCourse(Guid courseId)
    {
        return Ok();
    }

    // Hide a course
    [HttpPost("/hide/{courseId}")]
    public IActionResult HideCourse(Guid courseId)
    {
        return Ok();
    }

    // Get course by ID
    [HttpGet("/get/{courseId}")]
    public IActionResult GetCourseById(int courseId)
    {
        return Ok();
    }

    // Get courses by instructor
    [HttpGet("/get-by-instructor/{instructorId}")]
    public IActionResult GetCoursesByInstructor(Guid instructorId)
    {
        return Ok();
    }

    // Get course details
    [HttpGet("/details/{courseId}")]
    public IActionResult GetCourseDetails(Guid courseId)
    {
        return Ok();
    }

    // Get featured courses
    [HttpGet("/featured")]
    public IActionResult GetFeaturedCourses(EndpointFilter? filter)
    {
        return Ok();
    }

    // Get courses by keyword
    [HttpGet("/search")]
    public IActionResult SearchCourses([FromQuery] string? keyword, EndpointFilter filter)
    {
        return Ok();
    }

    // Get courses by keyword for search bar
    [HttpGet("/search-bar")]
    public IActionResult GetCoursesForSearchBar([FromQuery] string keyword, EndpointFilter filter)
    {
        return Ok();
    }
}