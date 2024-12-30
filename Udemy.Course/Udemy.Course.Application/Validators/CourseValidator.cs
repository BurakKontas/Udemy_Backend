﻿using FluentValidation;
using Udemy.Course.Domain.Entities;

namespace Udemy.Course.Application.Validators;

public class CourseValidator : AbstractValidator<Domain.Entities.Course>
{
    public CourseValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Course title is required.")
            .MaximumLength(100).WithMessage("Course title must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Course description is required.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");

        RuleFor(x => x.Level)
            .IsInEnum().WithMessage("Invalid course level.");

        RuleFor(x => x.Language)
            .NotEmpty().WithMessage("Language is required.");
    }
}