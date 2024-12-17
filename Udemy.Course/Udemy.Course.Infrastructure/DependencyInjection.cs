using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Udemy.Common.Helpers;
using Udemy.Course.Domain.Interfaces;
using Udemy.Course.Infrastructure.Contexts;
using Udemy.Course.Infrastructure.Repositories;

namespace Udemy.Course.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            var connectionString = PostgresConnectionOptions.FromEnvironment()
                .BuildConnectionString();
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
        services.AddScoped<ILessonRepository, LessonRepository>();
        services.AddScoped<IRateRepository, RateRepository>();

        return services;
    }
}