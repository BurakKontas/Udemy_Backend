using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Entities;
using Udemy.Course.Domain.Interfaces.Repository;
using Udemy.Course.Domain.Interfaces.Service;

namespace Udemy.Course.Application.Services;

public class QuestionService(IQuestionRepository questionRepository) : IQuestionService
{
    private readonly IQuestionRepository _questionRepository = questionRepository;

    public async Task<IEnumerable<Question>> GetAllAsync(EndpointFilter filter)
    {
        return await _questionRepository.GetAll(filter);
    }

    public async Task<Question> GetByIdAsync(Guid id)
    {
        var question = await _questionRepository.GetByIdAsync(id);

        if (question is null)
        {
            throw new KeyNotFoundException();
        }

        return question;
    }

    public async Task<Guid> AddAsync(Guid userId, Guid lessonId, string value)
    {
        var question = new Question
        {
            UserId = userId,
            LessonId = lessonId,
            Value = value
        };

        return await _questionRepository.AddAsync(question);
    }

    public async Task<Guid> UpdateAsync(Guid userId, Guid questionId, Dictionary<string, object> updates)
    {
        var question = await _questionRepository.GetByIdAsync(questionId);

        if (question is null)
        {
            throw new KeyNotFoundException();
        }

        if(question.UserId != userId)
        {
            throw new UnauthorizedAccessException();
        }

        var updated = await _questionRepository.UpdateAsync(question, updates);
        return updated.Id;
    }

    public async Task<Guid> DeleteAsync(Guid userId, Guid id)
    {
        var question = await _questionRepository.GetByIdAsync(id);

        if (question is null)
        {
            throw new KeyNotFoundException();
        }

        if(question.UserId != userId)
        {
            throw new UnauthorizedAccessException();
        }

        await _questionRepository.DeleteAsync(question);

        return question.Id;
    }

    public async Task<IEnumerable<Question>> GetQuestionsByLessonIdAsync(Guid lessonId, EndpointFilter filter)
    {
        return await _questionRepository.GetQuestionsByLessonIdAsync(lessonId, filter);
    }

    public async Task<IEnumerable<Question>> GetQuestionsByUserIdAsync(Guid userId, EndpointFilter filter)
    {
        return await _questionRepository.GetQuestionsByUserIdAsync(userId, filter);
    }
}