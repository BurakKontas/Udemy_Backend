using System.Linq.Expressions;
using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Entities;
using Udemy.Course.Domain.Interfaces.Repository;
using Udemy.Course.Domain.Interfaces.Service;

namespace Udemy.Course.Application.Services;

public class LessonService(ILessonRepository lessonRepository) : ILessonService
{
    private readonly ILessonRepository _lessonRepository = lessonRepository;

    public async Task<IEnumerable<Lesson>> GetAll(Guid categoryId, EndpointFilter filter)
    {
        return await _lessonRepository.GetAll(categoryId, filter);
    }

    public async Task<Guid> AddAsync(Guid userId, Guid categoryId, string title, string videoUrl, TimeSpan duration, string? description)
    {
        var lesson = new Lesson
        {
            LessonCategoryId = categoryId,
            Title = title,
            VideoUrl = videoUrl,
            Duration = duration,
            Description = description ?? string.Empty
        };

        return await _lessonRepository.AddAsync(userId, lesson, categoryId);
    }

    public async Task<Lesson?> GetByIdAsync(Guid id, Guid categoryId)
    {
        return await _lessonRepository.GetByIdAsync(id, categoryId);
    }

    public async Task<IEnumerable<Lesson>> GetManyAsync(Expression<Func<Lesson, bool>> predicate, Guid categoryId, EndpointFilter filter)
    {
        return await _lessonRepository.GetManyAsync(predicate, categoryId, filter);
    }

    public async Task<Guid> UpdateAsync(Guid userId, Guid id, Dictionary<string, object> updates)
    {
        var lesson = await _lessonRepository.GetByIdAsync(id);

        if (lesson == null)
        {
            throw new Exception("Lesson not found");
        }

        await _lessonRepository.UpdateAsync(userId, lesson, updates);

        return id;
    }

    public async Task<Guid> DeleteAsync(Guid userId, Guid id)
    {
        await _lessonRepository.DeleteAsync(userId, id);

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

    public async Task<IEnumerable<Lesson>> GetAllAsync(Guid categoryId, EndpointFilter filter)
    {
        return await _lessonRepository.GetAll(categoryId, filter);
    }

    public async Task<IEnumerable<Lesson>> GetByCourseIdAsync(Guid courseId, EndpointFilter filter)
    {
        var lessons = await _lessonRepository.GetManyAsync(x => x.CourseId == courseId, filter);
        return lessons;
    }

    public async Task<IEnumerable<Lesson>> GetByCategoryIdAsync(Guid categoryId, EndpointFilter filter)
    {
        return await _lessonRepository.GetManyAsync(x => x.LessonCategoryId == categoryId, filter);
    }
}