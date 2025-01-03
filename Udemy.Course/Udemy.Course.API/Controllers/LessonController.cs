using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Interfaces.Service;

namespace Udemy.Course.API.Controllers;

[Route("/v{version:apiVersion}/lesson")]
[ApiController]
[ApiVersion("1.0")]
public class LessonController(ILessonCategoryService lessonCategoryService, ILessonService lessonService) : ControllerBase
{
    private readonly ILessonCategoryService _lessonCategoryService = lessonCategoryService;
    private readonly ILessonService _lessonService = lessonService;

    // create lesson category
    [HttpPost("category/create")]
    public async Task<IResult> CreateLessonCategory([FromBody] CreateLessonCategoryRequest request)
    {
        var result = await _lessonCategoryService.AddAsync(request.CourseId, request.Name, request.Description);
        return TypedResults.Redirect($"category/{result}");
    }

    // get lesson category
    [HttpGet("category/{categoryId:guid}")]
    public async Task<IResult> GetLessonCategory(Guid categoryId)
    {
        var result = await _lessonCategoryService.GetByIdAsync(categoryId);
        return TypedResults.Ok(result);
    }

    [HttpGet("category/course/{courseId:guid}")]
    public async Task<IResult> GetLessonCategories(Guid courseId, EndpointFilter filter)
    {
        var result = await _lessonCategoryService.GetByCourseIdAsync(courseId, filter);
        return TypedResults.Ok(result);
    }

    // update lesson category
    [HttpPost("category/update/{categoryId:guid}")]
    public async Task<IResult> UpdateLessonCategory([FromBody] UpdateRequest request, Guid categoryId)
    {
        var result = await _lessonCategoryService.UpdateAsync(categoryId, request.Updates);
        return TypedResults.Redirect($"category/{result}");
    }

    // delete lesson category
    [HttpDelete("category/delete/{categoryId:guid}")]
    public async Task<IResult> DeleteLessonCategory(Guid categoryId)
    {
        await _lessonCategoryService.DeleteAsync(categoryId);
        return TypedResults.NoContent();
    }

    // create lesson
    [HttpPost("create")]
    public async Task<IResult> CreateLesson([FromBody] CreateLessonRequest request)
    {
        var result = await _lessonService.AddAsync(request.CategoryId, request.Title, request.VideoUrl, request.Duration, request.Description);
        return TypedResults.Redirect($"get/{result}");
    }

    // get lesson
    [HttpGet("get/{lessonId:guid}")]
    public async Task<IResult> GetLesson(Guid lessonId)
    {
        var result = await _lessonService.GetByIdAsync(lessonId);
        return TypedResults.Ok(result);
    }

    // update lesson
    [HttpPost("update/{lessonId:guid}")]
    public async Task<IResult> UpdateLesson([FromBody] UpdateRequest request, Guid lessonId)
    {
        var result = await _lessonService.UpdateAsync(lessonId, request.Updates);
        return TypedResults.Redirect($"get/{result}");
    }

    // delete lesson
    [HttpDelete("delete/{lessonId:guid}")]
    public async Task<IResult> DeleteLesson(Guid lessonId)
    {
        await _lessonService.DeleteAsync(lessonId);
        return TypedResults.NoContent();
    }

    // get lessons by course
    [HttpGet("course/{courseId:guid}")]
    public async Task<IResult> GetLessonsByCourse(Guid courseId, EndpointFilter filter)
    {
        var result = await _lessonService.GetByCourseIdAsync(courseId, filter);
        return TypedResults.Ok(result);
    }

    // get lessons by category
    [HttpGet("category/lesson/{categoryId:guid}")]
    public async Task<IResult> GetLessonsByCategory(Guid categoryId, EndpointFilter filter)
    {
        var result = await _lessonService.GetByCategoryIdAsync(categoryId, filter);
        return TypedResults.Ok(result);
    }
}

public record CreateLessonCategoryRequest(Guid CourseId, string Name, string? Description);
public record UpdateRequest(Dictionary<string, object> Updates);
public record CreateLessonRequest(Guid CategoryId, string Title, string VideoUrl, TimeSpan Duration, string? Description);