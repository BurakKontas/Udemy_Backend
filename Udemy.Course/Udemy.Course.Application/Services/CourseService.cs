﻿using FluentValidation;
using Udemy.Common.Exceptions;
using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Entities;
using Udemy.Course.Domain.Enums;
using Udemy.Course.Domain.Interfaces.Repository;
using Udemy.Course.Domain.Interfaces.Service;
using AppCourse = Udemy.Course.Domain.Entities.Course;

namespace Udemy.Course.Application.Services;

public class CourseService(IValidator<AppCourse> validator, ICourseRepository courseRepository) : ICourseService
{
    private readonly IValidator<AppCourse> _validator = validator;
    private readonly ICourseRepository _courseRepository = courseRepository;

    public async Task<Guid> CreateAsync(Guid instructorId, string title, string description, decimal price, CourseLevel level, string language, bool isActive)
    {
        var course = AppCourse.Create(instructorId, title, level, language, isActive);
        var courseDetails = new CourseDetails()
        {
            Description = description,
            Price = price,
            CourseId = course.Id
        };

        course.SetDetails(courseDetails);

        await ValidateCourse(course);
        await _courseRepository.AddAsync(course);

        return course.Id;
    }

    public async Task<Guid> UpdateAsync(Guid userId, Guid courseId, Dictionary<string, object> updates)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null)
            throw new NotFoundException($"Course with id {courseId} not found");

        if (!course.InstructorIds.Contains(userId))
            throw new UnauthorizedAccessException("You are not authorized to update this course");

        var originalCourse = course.Clone() as AppCourse;

        try
        {
            await _courseRepository.UpdateAsync(course, updates);

            await ValidateCourse(course);

            return courseId;
        }
        catch (ValidationException)
        {
            await _courseRepository.UpdateAsync(originalCourse!, updates);

            throw new ValidationException("Course update failed due to validation errors.");
        }
    }

    public async Task<Guid> DeleteAsync(Guid userId, Guid courseId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null)
            throw new NotFoundException($"Course with id {courseId} not found");

        if (course.OwnerId != userId)
            throw new UnauthorizedAccessException("You are not authorized to delete this course");

        await _courseRepository.DeleteAsync(course);

        return courseId;
    }

    public async Task<AppCourse?> GetByIdAsync(Guid courseId)
    {
        return await _courseRepository.GetByIdAsync(courseId);
    }

    public async Task<IEnumerable<AppCourse>> GetAllByInstructorAsync(Guid instructorId, EndpointFilter filter)
    {
        return await _courseRepository.GetCoursesByInstructorAsync(instructorId, filter);
    }

    public async Task<IEnumerable<AppCourse>> SearchCoursesAsync(string keyword, EndpointFilter filter)
    {
        return await _courseRepository.GetCoursesByKeywordAsync(keyword, filter);
    }

    public async Task<IEnumerable<AppCourse>> GetFeaturedCoursesAsync(EndpointFilter filter)
    {
        return await _courseRepository.GetFeaturedCoursesAsync(filter);
    }

    public async Task<Guid> AssignInstructorAsync(Guid courseId, Guid instructorId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null)
            throw new NotFoundException($"Course with id {courseId} not found");

        course.AssignInstructor(instructorId);
        await _courseRepository.UpdateAsync(course);

        return courseId;
    }

    public async Task<Guid> UpdateStatusAsync(Guid userId, Guid courseId, bool isActive)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null)
            throw new NotFoundException($"Course with id {courseId} not found");

        if (!course.InstructorIds.Contains(userId))
            throw new UnauthorizedAccessException("You are not authorized to update this course");

        course.UpdateStatus(isActive);
        await _courseRepository.UpdateAsync(course);

        return courseId;
    }

    public async Task<AppCourse[]> GetAllCourses(EndpointFilter filter)
    {
        var courses = await _courseRepository.GetAll(filter);
        return courses.ToArray();
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