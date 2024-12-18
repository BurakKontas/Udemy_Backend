using FluentValidation;
using Udemy.Common.Exceptions;
using Udemy.Course.Domain.Enums;
using Udemy.Course.Domain.Interfaces.Repository;
using Udemy.Course.Domain.Interfaces.Service;
using AppCourse = Udemy.Course.Domain.Entities.Course;

namespace Udemy.Course.Application.Services;

public class CourseService(IValidator<AppCourse> validator, ICourseRepository courseRepository) : ICourseService
{
    private readonly IValidator<AppCourse> _validator = validator;
    private readonly ICourseRepository _courseRepository = courseRepository;

    public async Task CreateAsync(Guid instructorId, string title, string description, decimal price, CourseLevel level, string language, bool isActive)
    {
        var course = AppCourse.Create(title, description, price, level, language, isActive);
        course.AssignInstructor(instructorId);

        await ValidateCourse(course);
        await _courseRepository.AddAsync(course);
    }

    public async Task UpdateAsync(Guid courseId, Dictionary<string, object> updates)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null)
            throw new NotFoundException($"Course with id {courseId} not found");

        var originalCourse = course.Clone() as AppCourse;

        try
        {
            await _courseRepository.UpdateAsync(course, updates);

            await ValidateCourse(course);
        }
        catch (ValidationException)
        {
            await _courseRepository.UpdateAsync(originalCourse!, updates);

            throw new ValidationException("Course update failed due to validation errors.");
        }
    }

    public async Task DeleteAsync(Guid courseId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null)
            throw new NotFoundException($"Course with id {courseId} not found");

        await _courseRepository.DeleteAsync(course);
    }

    public async Task<AppCourse?> GetByIdAsync(Guid courseId)
    {
        return await _courseRepository.GetByIdAsync(courseId);
    }

    public async Task<IEnumerable<AppCourse>> GetAllByInstructorAsync(Guid instructorId)
    {
        return await _courseRepository.GetCoursesByInstructorAsync(instructorId);
    }

    public async Task<IEnumerable<AppCourse>> SearchCoursesAsync(string keyword)
    {
        return await _courseRepository.GetCoursesByKeywordAsync(keyword);
    }

    public async Task<IEnumerable<AppCourse>> GetFeaturedCoursesAsync()
    {
        return await _courseRepository.GetFeaturedCoursesAsync();
    }

    public async Task AssignInstructorAsync(Guid courseId, Guid instructorId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null)
            throw new NotFoundException($"Course with id {courseId} not found");

        course.AssignInstructor(instructorId);
        await _courseRepository.UpdateAsync(course);
    }

    public async Task UpdateStatusAsync(Guid courseId, bool isActive)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null)
            throw new NotFoundException($"Course with id {courseId} not found");

        course.UpdateStatus(isActive);
        await _courseRepository.UpdateAsync(course);
    }

    private async Task ValidateCourse(AppCourse course)
    {
        var validationResult = await _validator.ValidateAsync(course);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
    }
}