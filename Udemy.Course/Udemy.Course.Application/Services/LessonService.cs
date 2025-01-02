using System.Linq.Expressions;
using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Entities;
using Udemy.Course.Domain.Interfaces.Repository;
using Udemy.Course.Domain.Interfaces.Service;

namespace Udemy.Course.Application.Services;

public class LessonService(ILessonRepository lessonRepository) : ILessonService
{
    private readonly ILessonRepository _lessonRepository = lessonRepository;

    public async Task<Guid> AddAsync(Lesson entity)
    {
        var categoryId = entity.LessonCategoryId;
        return await _lessonRepository.AddAsync(entity, categoryId);
    }

    public async Task<IEnumerable<Lesson>> GetAll(Guid categoryId, EndpointFilter filter)
    {
        return await _lessonRepository.GetAll(categoryId, filter);
    }

    public async Task<Lesson?> GetByIdAsync(Guid id, Guid categoryId)
    {
        return await _lessonRepository.GetByIdAsync(id, categoryId);
    }

    public async Task<IEnumerable<Lesson>> GetManyAsync(Expression<Func<Lesson, bool>> predicate, Guid categoryId, EndpointFilter filter)
    {
        return await _lessonRepository.GetManyAsync(predicate, categoryId, filter);
    }

    public async Task<Guid> UpdateAsync(Guid id, Dictionary<string, object> updates)
    {
        var lesson = await _lessonRepository.GetByIdAsync(id);

        if (lesson == null)
        {
            throw new Exception("Lesson not found");
        }

        await _lessonRepository.UpdateAsync(lesson, updates);

        return id;
    }

    public async Task<Guid> DeleteAsync(Guid id)
    {
        var lesson = await _lessonRepository.GetByIdAsync(id);

        if (lesson == null)
        {
            throw new Exception("Lesson not found");
        }

        await _lessonRepository.DeleteAsync(lesson);

        return id;
    }

    public async Task<IEnumerable<Lesson>> GetAllAsync(EndpointFilter filter)
    {
        return await _lessonRepository.GetAll(filter);
    }

    public async Task<Lesson?> GetByIdAsync(Guid id)
    {
        return await _lessonRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Lesson>> GetManyAsync(Expression<Func<Lesson, bool>> predicate, EndpointFilter filter)
    {
        return await _lessonRepository.GetManyAsync(predicate, filter);
    }

    public async Task<Guid> AddAsync(Lesson entity, Guid categoryId)
    {
        return await _lessonRepository.AddAsync(entity, categoryId);
    }

    public async Task<IEnumerable<Lesson>> GetAllAsync(Guid categoryId, EndpointFilter filter)
    {
        return await _lessonRepository.GetAll(categoryId, filter);
    }
}