using Asp.Versioning;
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
    [HttpPost("/{courseId:guid}")]
    public async Task<IResult> MakeComment([FromBody] AddCommentRequest request, Guid courseId)
    {
        var commentId = await _commentService.AddAsync(request.UserId, courseId, request.Value, request.Rate);

        return TypedResults.Redirect($"get/{commentId}");
    }

    // edit comments
    [HttpPost("/update/{commentId:guid}")]
    public async Task<IResult> EditComment(Guid commentId, [FromBody] UpdateRequest request)
    {
        var result = await _commentService.UpdateAsync(commentId, request.Updates);

        return TypedResults.Redirect($"get/{result}");
    }

    // delete comment
    [HttpDelete("/delete/{commentId:guid}")]
    public async Task<IResult> DeleteComment(Guid commentId)
    {
        await _commentService.DeleteAsync(commentId);

        return TypedResults.NoContent();
    }
}

public record AddCommentRequest(Guid UserId, string Value, int Rate);