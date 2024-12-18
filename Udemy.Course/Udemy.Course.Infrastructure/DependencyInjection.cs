using Elastic.Clients.Elasticsearch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Udemy.Common.Helpers;
using Udemy.Course.Domain.Interfaces.Repository;
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

        services.AddSingleton<ElasticsearchClient>(serviceProvider =>
        {
            var url = Environment.GetEnvironmentVariable("ELASTICSEARCH_URL") ?? "http://localhost:9200";
            var options = new Uri(url);
            var client = new ElasticsearchClient(options);
            return client;
        });

        services.AddScoped<IElasticSearchRepository, ElasticSearchRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
        services.AddScoped<ILessonRepository, LessonRepository>();
        services.AddScoped<IRateRepository, RateRepository>();

        return services;
    }
}