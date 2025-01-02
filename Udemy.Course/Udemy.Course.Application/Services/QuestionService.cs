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

    public async Task<Guid> AddAsync(Question question)
    {
        return await _questionRepository.AddAsync(question);
    }

    public async Task<Guid> UpdateAsync(Question question, Dictionary<string, object> updates)
    {
        var updated = await _questionRepository.UpdateAsync(question, updates);
        return updated.Id;
    }

    public async Task<Guid> DeleteAsync(Guid id)
    {
        var question = await _questionRepository.GetByIdAsync(id);

        if (question is null)
        {
            throw new KeyNotFoundException();
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