using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Udemy.Common.ModelBinder;
using Udemy.Course.Contracts.Requests;
using Udemy.Course.Domain.Enums;
using Udemy.Course.Domain.Interfaces.Service;

namespace Udemy.Course.API.Controllers;

[Route("/")]
[ApiController]
public class CourseController(ICourseService courseService) : ControllerBase
{
    private readonly ICourseService _courseService = courseService;

    // Get all courses (only for debug purposes)
    [HttpGet("/get-all")]
    public async Task<IResult> GetAllCourses(EndpointFilter filter)
    {
        var result = await _courseService.GetAllCourses(filter);
        return TypedResults.Ok(result);
    }

    // Create a course
    [HttpPost("/create")]
    public async Task<IResult> CreateCourse([FromBody] CreateCourseRequest request)
    {
        var isLevelValid = Enum.TryParse(request.Level, true, out CourseLevel courseLevel);

        if (!isLevelValid) throw new InvalidEnumArgumentException("CreateCourseRequest.Level");

        var id = await _courseService.CreateAsync(request.InstructorId, request.Title, request.Description, request.Price, courseLevel, request.Language, request.IsActive);

        return TypedResults.Redirect($"http://localhost:3000/api/course/get/{id}");
    }

    // Update a course
    [HttpPost("/update/{courseId}")]
    public async Task<IResult> UpdateCourse([FromBody] UpdateCourseRequest request, Guid courseId)
    {
        var id = await _courseService.UpdateAsync(courseId, request.Updates);

        return TypedResults.Redirect($"http://localhost:3000/api/course/get/{id}");
    }

    // Delete a course
    [HttpDelete("/delete/{courseId}")]
    public async Task<IResult> DeleteCourse(Guid courseId)
    {
        var id = await _courseService.DeleteAsync(courseId);

        return TypedResults.Ok(id);
    }

    // Hide a course
    [HttpPost("/update-status/{courseId}")]
    public async Task<IResult> HideCourse([FromBody] UpdateCourseStatusRequest request, Guid courseId)
    {
        var id = await _courseService.UpdateStatusAsync(courseId, request.IsActive);

        return TypedResults.Ok(id);
    }

    // Get course by ID
    [HttpGet("/get/{courseId}")]
    public async Task<IResult> GetCourseById(Guid courseId)
    {
        var result = await _courseService.GetByIdAsync(courseId);

        if(result is null) return TypedResults.NotFound();

        return TypedResults.Ok(result);
    }

    // Get courses by instructor
    [HttpGet("/get-by-instructor/{instructorId}")]
    public async Task<IResult> GetCoursesByInstructor(Guid instructorId, EndpointFilter filter)
    {
        var result = await _courseService.GetAllByInstructorAsync(instructorId, filter);

        var courses = result as Domain.Entities.Course[] ?? result.ToArray();
        if(!courses.Any()) return TypedResults.NoContent();

        return TypedResults.Ok(courses);
    }

    // Get course details
    // TODO: Refactor this later
    [HttpGet("/details/{courseId}")]
    public async Task<IResult> GetCourseDetails(Guid courseId)
    {
        var result = await _courseService.GetByIdAsync(courseId);

        if (result is null) return TypedResults.NotFound();

        return TypedResults.Ok(result);
    }

    // Get featured courses
    [HttpGet("/featured")]
    public async Task<IResult> GetFeaturedCourses(EndpointFilter filter)
    {
        var result = await _courseService.GetFeaturedCoursesAsync(filter);

        var courses = result as Domain.Entities.Course[] ?? result.ToArray();
        if (!courses.Any()) return TypedResults.NoContent();

        return TypedResults.Ok(courses);
    }

    // Get courses by keyword
    [HttpGet("/search")]
    public async Task<IResult> SearchCourses([FromQuery] string? keyword, EndpointFilter filter)
    {
        keyword ??= "";
        var result = await _courseService.SearchCoursesAsync(keyword, filter);

        var courses = result as Domain.Entities.Course[] ?? result.ToArray();
        if (!courses.Any()) return TypedResults.NoContent();

        return TypedResults.Ok(courses);
    }

    // Get courses by keyword for search bar
    [HttpGet("/search-bar")]
    public async Task<IResult> GetCoursesForSearchBar([FromQuery] string keyword, EndpointFilter filter)
    {
        keyword ??= "";
        var result = await _courseService.SearchCoursesAsync(keyword, filter);

        var courses = result as Domain.Entities.Course[] ?? result.ToArray();
        if (!courses.Any()) return TypedResults.NoContent();

        return TypedResults.Ok(courses);
    }
}