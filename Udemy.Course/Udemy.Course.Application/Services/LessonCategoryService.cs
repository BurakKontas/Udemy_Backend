using System.Linq.Expressions;
using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Entities;
using Udemy.Course.Domain.Interfaces.Repository;
using Udemy.Course.Domain.Interfaces.Service;

namespace Udemy.Course.Application.Services;

public class LessonCategoryService(ILessonCategoryRepository lessonCategoryRepository) : ILessonCategoryService
{
    private readonly ILessonCategoryRepository _lessonCategoryRepository = lessonCategoryRepository;

    public async Task<Guid> AddAsync(LessonCategory entity)
    {
        var courseId = entity.CourseId;
        return await _lessonCategoryRepository.AddAsync(entity, courseId);
    }

    public async Task<IEnumerable<LessonCategory>> GetAll(Guid courseId, EndpointFilter filter)
    {
        return await _lessonCategoryRepository.GetAll(courseId, filter);
    }

    public async Task<LessonCategory?> GetByIdAsync(Guid id, Guid courseId)
    {
        return await _lessonCategoryRepository.GetByIdAsync(id, courseId);
    }

    public async Task<IEnumerable<LessonCategory>> GetManyAsync(Expression<Func<LessonCategory, bool>> predicate, Guid courseId, EndpointFilter filter)
    {
        return await _lessonCategoryRepository.GetManyAsync(predicate, courseId, filter);
    }

    public async Task<Guid> UpdateAsync(Guid id, Dictionary<string, object> updates)
    {
        var lessonCategory = await _lessonCategoryRepository.GetByIdAsync(id);

        if (lessonCategory == null)
        {
            throw new Exception("LessonCategory not found");
        }

        await _lessonCategoryRepository.UpdateAsync(lessonCategory, updates);

        return id;
    }

    public async Task<Guid> DeleteAsync(Guid id)
    {
        var lessonCategory = await _lessonCategoryRepository.GetByIdAsync(id);

        if (lessonCategory == null)
        {
            throw new Exception("LessonCategory not found");
        }

        await _lessonCategoryRepository.DeleteAsync(lessonCategory);

        return id;
    }

    public async Task<IEnumerable<LessonCategory>> GetAllAsync(EndpointFilter filter)
    {
        return await _lessonCategoryRepository.GetAll(filter);
    }

    public async Task<LessonCategory?> GetByIdAsync(Guid id)
    {
        return await _lessonCategoryRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<LessonCategory>> GetManyAsync(Expression<Func<LessonCategory, bool>> predicate, EndpointFilter filter)
    {
        return await _lessonCategoryRepository.GetManyAsync(predicate, filter);
    }

    public async Task<Guid> AddAsync(LessonCategory entity, Guid courseId)
    {
        return await _lessonCategoryRepository.AddAsync(entity, courseId);
    }

    public async Task<IEnumerable<LessonCategory>> GetAllAsync(Guid courseId, EndpointFilter filter)
    {
        return await _lessonCategoryRepository.GetAll(courseId, filter);
    }
}