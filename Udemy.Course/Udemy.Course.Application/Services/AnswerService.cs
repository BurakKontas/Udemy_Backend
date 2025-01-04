using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Entities;
using Udemy.Course.Domain.Interfaces.Repository;
using Udemy.Course.Domain.Interfaces.Service;

namespace Udemy.Course.Application.Services;

public class AnswerService(IAnswerRepository answerRepository) : IAnswerService
{
    private readonly IAnswerRepository _answerRepository = answerRepository;

    public async Task<IEnumerable<Answer>> GetAllAsync(EndpointFilter filter)
    {
        return await _answerRepository.GetAll(filter);
    }

    public async Task<Answer> GetByIdAsync(Guid id)
    {
        var answer = await _answerRepository.GetByIdAsync(id);

        if (answer is null)
        {
            throw new KeyNotFoundException();
        }

        return answer;
    }

    public async Task<Guid> AddAsync(Guid userId, Guid questionId, string value)
    {
        var answer = new Answer
        {
            UserId = userId,
            QuestionId = questionId,
            Value = value
        };

        return await _answerRepository.AddAsync(answer);
    }

    public async Task<Guid> UpdateAsync(Guid answerId, Dictionary<string, object> updates)
    {
        var answer = await _answerRepository.GetByIdAsync(answerId);

        if (answer is null)
        {
            throw new KeyNotFoundException();
        }

        var updated = await _answerRepository.UpdateAsync(answer, updates);
        return updated.Id;
    }

    public async Task<Guid> DeleteAsync(Guid id)
    {
        var answer = await _answerRepository.GetByIdAsync(id);

        if (answer is null)
        {
            throw new KeyNotFoundException();
        }

        await _answerRepository.DeleteAsync(answer);

        return answer.Id;
    }

    public async Task<IEnumerable<Answer>> GetAnswersByQuestionIdAsync(Guid questionId, EndpointFilter filter)
    {
        return await _answerRepository.GetAnswersByQuestionIdAsync(questionId, filter);
    }

    public async Task<IEnumerable<Answer>> GetAnswersByUserIdAsync(Guid userId, EndpointFilter filter)
    {
        return await _answerRepository.GetAnswersByUserIdAsync(userId, filter);
    }

    public async Task<IEnumerable<Answer>> GetAnswersByQuestionIdAndUserIdAsync(Guid questionId, Guid userId, EndpointFilter filter)
    {
        return await _answerRepository.GetAnswersByQuestionIdAndUserIdAsync(questionId, userId, filter);
    }

    public async Task<Guid> AddAsync(Guid questionId, Answer answer)
    {
        return await _answerRepository.AddAsync(questionId, answer);
    }

    public async Task DeleteAsync(Guid questionId, Guid answerId)
    {
        await _answerRepository.DeleteAsync(questionId, answerId);
    }
}