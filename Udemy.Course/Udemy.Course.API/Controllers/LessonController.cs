using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    [HttpPost("category/create")]
    public async Task<IResult> CreateLessonCategory([FromBody] CreateLessonCategoryRequest request, UserId userId)
    {
        var result = await _lessonCategoryService.AddAsync(userId.Value, request.CourseId, request.Name, request.Description);
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
    [Authorize]
    [HttpPost("category/update/{categoryId:guid}")]
    public async Task<IResult> UpdateLessonCategory([FromBody] UpdateRequest request, Guid categoryId, UserId userId)
    {
        var result = await _lessonCategoryService.UpdateAsync(userId.Value, categoryId, request.Updates);
        return TypedResults.Redirect($"category/{result}");
    }

    // delete lesson category
    [Authorize]
    [HttpDelete("category/delete/{categoryId:guid}")]
    public async Task<IResult> DeleteLessonCategory(Guid categoryId, UserId userId)
    {
        await _lessonCategoryService.DeleteAsync(userId.Value, categoryId);
        return TypedResults.NoContent();
    }

    // create lesson
    [Authorize]
    [HttpPost("create")]
    public async Task<IResult> CreateLesson([FromBody] CreateLessonRequest request, UserId userId)
    {
        var result = await _lessonService.AddAsync(userId.Value, request.CategoryId, request.Title, request.VideoUrl, request.Duration, request.Description);
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
    [Authorize]
    [HttpPost("update/{lessonId:guid}")]
    public async Task<IResult> UpdateLesson([FromBody] UpdateRequest request, Guid lessonId, UserId userId)
    {
        var result = await _lessonService.UpdateAsync(userId.Value, lessonId, request.Updates);
        return TypedResults.Redirect($"get/{result}");
    }

    // delete lesson
    [Authorize]
    [HttpDelete("delete/{lessonId:guid}")]
    public async Task<IResult> DeleteLesson(Guid lessonId, UserId userId)
    {
        await _lessonService.DeleteAsync(userId.Value, lessonId);
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