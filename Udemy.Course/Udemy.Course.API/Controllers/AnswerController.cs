using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Interfaces.Service;

namespace Udemy.Course.API.Controllers;

[Route("/v{version:apiVersion}/answer")]
[ApiController]
[ApiVersion("1.0")]
public class AnswerController(IAnswerService answerService) : ControllerBase
{
    private readonly IAnswerService _answerService = answerService;

    // create answer
    [Authorize]
    [HttpPost("/create/{questionId:guid}")]
    public async Task<IResult> CreateAnswer([FromBody] CreateAnswerRequest request, Guid questionId)
    {
        var result = await _answerService.AddAsync(request.QuestionId, request.UserId, request.Content);
        return TypedResults.Redirect($"get/{result}");
    }

    // get answer
    [HttpGet("/get/{answerId:guid}")]
    public async Task<IResult> GetAnswer(Guid answerId)
    {
        var result = await _answerService.GetByIdAsync(answerId);

        return TypedResults.Ok(result);
    }

    // update answer
    [Authorize]
    [HttpPost("/update/{answerId:guid}")]
    public async Task<IResult> UpdateAnswer(Guid answerId, [FromBody] UpdateRequest request)
    {
        var result = await _answerService.UpdateAsync(answerId, request.Updates);

        return TypedResults.Redirect($"get/{result}");
    }

    // delete answer
    [Authorize]
    [HttpDelete("/delete/{answerId:guid}")]
    public async Task<IResult> DeleteAnswer(Guid answerId)
    {
        await _answerService.DeleteAsync(answerId);

        return TypedResults.NoContent();
    }

    // get answers by question
    [HttpGet("/question/{questionId:guid}")]
    public async Task<IResult> GetAnswersByQuestion(Guid questionId, EndpointFilter filter)
    {
        var result = await _answerService.GetAnswersByQuestionIdAsync(questionId, filter);

        return TypedResults.Ok(result);
    }

    // get answers by user
    [HttpGet("/user/{userId:guid}")]
    public async Task<IResult> GetAnswersByUser(Guid userId, EndpointFilter filter)
    {
        var result = await _answerService.GetAnswersByUserIdAsync(userId, filter);

        return TypedResults.Ok(result);
    }

    // get answer by user and question
    [HttpGet("/question/{questionId:guid}/user/{userId:guid}")]
    public async Task<IResult> GetAnswersByQuestionIdAndUserId(Guid questionId, Guid userId, EndpointFilter filter)
    {
        var result = await _answerService.GetAnswersByQuestionIdAndUserIdAsync(questionId, userId, filter);

        return TypedResults.Ok(result);
    }
}

public record CreateAnswerRequest(Guid QuestionId, Guid UserId, string Content);