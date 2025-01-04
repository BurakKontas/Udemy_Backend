﻿using Asp.Versioning;
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
    [HttpPost("/{question:guid}")]
    public async Task<IResult> LikeQuestion([FromBody] LikeRequest request, Guid questionId)
    {
        var result = await _likeService.AddAsync(request.UserId, questionId);
        return TypedResults.Redirect($"get/{result}");
    }

    // unlike question
    [HttpDelete("/{likeId:guid}")]
    public async Task<IResult> UnlikeQuestion(Guid likeId)
    {
        var result = await _likeService.DeleteAsync(likeId);

        return TypedResults.NoContent();
    }

    // get like
    [HttpGet("/{likeId:guid}")]
    public async Task<IResult> GetLike(Guid likeId)
    {
        var result = await _likeService.GetByIdAsync(likeId);

        return TypedResults.Ok(result);
    }

    // get likes by user
    [HttpGet("/user/{userId:guid}")]
    public async Task<IResult> GetLikesByUser(Guid userId, EndpointFilter filter)
    {
        var result = await _likeService.GetLikesByUserIdAsync(userId, filter);

        return TypedResults.Ok(result);
    }

    // get likes by question
    [HttpGet("/question/{questionId:guid}")]
    public async Task<IResult> GetLikesByQuestion(Guid questionId, EndpointFilter filter)
    {
        var result = await _likeService.GetLikesByQuestionIdAsync(questionId, filter);

        return TypedResults.Ok(result);
    }
}

public record LikeRequest(Guid UserId);