using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Entities;
using Udemy.Course.Domain.Interfaces.Service;

namespace Udemy.Course.API.Controllers;

[Route("/v{version:apiVersion}/question")]
[ApiController]
[ApiVersion("1.0")]
public class QuestionController(IQuestionService questionService) : ControllerBase
{
    private readonly IQuestionService _questionService = questionService;

    // get questions by lesson
    [HttpGet("/lesson/{lessonId:guid}")]
    public async Task<IResult> GetQuestionsByLesson(Guid lessonId, EndpointFilter filter)
    {
        var questions = await _questionService.GetQuestionsByLessonIdAsync(lessonId, filter);

        var questionsArray = questions as Question[] ?? questions.ToArray();
        if (!questionsArray.Any())
            return TypedResults.NoContent();

        return TypedResults.Ok(questionsArray);
    }

    // get question details
    [HttpGet("/get/{questionId:guid}")]
    public async Task<IResult> GetQuestionDetails(Guid questionId)
    {
        var question = await _questionService.GetByIdAsync(questionId);

        return TypedResults.Ok(question);
    }

    // make question
    [Authorize]
    [HttpPost]
    public async Task<IResult> MakeQuestion([FromBody] CreateQuestionRequest request)
    {
        var result = await _questionService.AddAsync(request.UserId, request.LessonId, request.Value);

        return TypedResults.Redirect($"get/{result}");
    }

    // edit question
    [Authorize]
    [HttpPost("/update/{questionId:guid}")]
    public async Task<IResult> EditQuestion(Guid questionId, [FromBody] UpdateRequest request)
    {
        var result = await _questionService.UpdateAsync(questionId, request.Updates);

        return TypedResults.Redirect($"get/{result}");
    }

    // delete question
    [Authorize]
    [HttpDelete("/delete/{questionId:guid}")]
    public async Task<IResult> DeleteQuestion(Guid questionId)
    {
        await _questionService.DeleteAsync(questionId);

        return TypedResults.NoContent();
    }

}

public record CreateQuestionRequest(Guid UserId, Guid LessonId, string Value);