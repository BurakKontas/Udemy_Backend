﻿using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;
using Udemy.Course.Application.Services;
using Udemy.Course.Application.Validators;
using Udemy.Course.Domain.Interfaces.Service;

namespace Udemy.Course.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(DependencyInjection)));

        services.AddScoped<IRateService, RateService>();
        services.AddScoped<ILessonService, LessonService>();
        services.AddScoped<IEnrollmentService, EnrollmentService>();
        services.AddScoped<ICommentService, CommentService>();

        return services;
    }
}