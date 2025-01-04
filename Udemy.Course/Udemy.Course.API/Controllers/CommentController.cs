using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Interfaces.Service;

namespace Udemy.Course.API.Controllers;

[Route("/v{version:apiVersion}/comment")]
[ApiController]
[ApiVersion("1.0")]
public class CommentController(ICommentService commentService) : ControllerBase
{
    private readonly ICommentService _commentService = commentService;

    // get comments by course
    [HttpGet("/course/{courseId:guid}")]
    public async Task<IResult> GetCommentsByCourse(Guid courseId, EndpointFilter filter)
    {
        var comments = await _commentService.GetCommentsByCourseIdAsync(courseId, filter);

        return TypedResults.Ok(comments);
    }

    // make comment
    [Authorize]
    [HttpPost("/{courseId:guid}")]
    public async Task<IResult> MakeComment([FromBody] AddCommentRequest request, Guid courseId, UserId userId)
    {
        var commentId = await _commentService.AddAsync(userId.Value, courseId, request.Value, request.Rate);

        return TypedResults.Redirect($"get/{commentId}");
    }

    // edit comments (to edit rate updates.rate.value)
    [Authorize]
    [HttpPost("/update/{commentId:guid}")]
    public async Task<IResult> EditComment(Guid commentId, [FromBody] UpdateRequest request, UserId userId)
    {
        var result = await _commentService.UpdateAsync(userId.Value, commentId, request.Updates);

        return TypedResults.Redirect($"get/{result}");
    }

    // delete comment
    [Authorize]
    [HttpDelete("/delete/{commentId:guid}")]
    public async Task<IResult> DeleteComment(Guid commentId, UserId userId)
    {
        await _commentService.DeleteAsync(userId.Value, commentId);

        return TypedResults.NoContent();
    }
}

public record AddCommentRequest(string Value, int Rate);