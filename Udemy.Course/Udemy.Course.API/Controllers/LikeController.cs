using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Entities;
using Udemy.Course.Domain.Interfaces.Repository;
using Udemy.Course.Domain.Interfaces.Service;

namespace Udemy.Course.API.Controllers;

[Route("/v{version:apiVersion}/like")]
[ApiController]
[ApiVersion("1.0")]
public class LikeController(ILikeService likeService) : ControllerBase
{
    private readonly ILikeService _likeService = likeService;

    // like question
    [Authorize]
    [HttpPost("/{question:guid}")]
    public async Task<IResult> LikeQuestion(UserId userId, Guid questionId)
    {
        var result = await _likeService.AddAsync(userId.Value, questionId);
        return TypedResults.Redirect($"get/{result}");
    }

    // unlike question
    [Authorize]
    [HttpDelete("/{likeId:guid}")]
    public async Task<IResult> UnlikeQuestion(Guid likeId, UserId userId)
    {
        var result = await _likeService.DeleteAsync(likeId);

        return TypedResults.NoContent();
    }

    // get like
    [Authorize]
    [HttpGet("/{likeId:guid}")]
    public async Task<IResult> GetLike(Guid likeId, UserId userId)
    {
        var result = await _likeService.GetByIdAsync(likeId);

        return TypedResults.Ok(result);
    }

    // get likes by user
    [Authorize]
    [HttpGet("/user")]
    public async Task<IResult> GetLikesByUser(UserId userId, EndpointFilter filter)
    {
        var result = await _likeService.GetLikesByUserIdAsync(userId.Value, filter);

        return TypedResults.Ok(result);
    }

    // get likes by question
    [Authorize]
    [HttpGet("/question/{questionId:guid}")]
    public async Task<IResult> GetLikesByQuestion(Guid questionId, EndpointFilter filter)
    {
        var result = await _likeService.GetLikesByQuestionIdAsync(questionId, filter);

        return TypedResults.Ok(result);
    }

    // get like count
    [HttpGet("/count/{questionId:guid}")]
    public async Task<IResult> GetLikeCount(Guid questionId)
    {
        var result = await _likeService.GetLikesCount(questionId);

        return TypedResults.Ok(result);
    }
}