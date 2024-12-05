using FluentValidation;

namespace Udemy.Auth.Domain.Options
{
    public class PostgresConnectionOptions
    {
        public string Host { get; private init; } = null!;
        public string Port { get; private init; } = null!;
        public string Database { get; private init; } = null!;
        public string Username { get; private init; } = null!;
        public string Password { get; private init; } = null!;

        private PostgresConnectionOptions() { }

        public string BuildConnectionString()
        {
            var connectionStringBuilder = new System.Text.StringBuilder();
            connectionStringBuilder.Append($"Host={Host};")
                .Append($"Port={Port};")
                .Append($"Database={Database};")
                .Append($"Username={Username};")
                .Append($"Password={Password};");
            return connectionStringBuilder.ToString();
        }

        public static PostgresConnectionOptions FromEnvironment()
        {
            return new PostgresConnectionOptions
            {
                Host = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? "localhost",
                Port = Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? "5432",
                Database = Environment.GetEnvironmentVariable("POSTGRES_DB") ?? "udemydb",
                Username = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? "udemyuser",
                Password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? "udemypassword"
            };
        }
    }
}