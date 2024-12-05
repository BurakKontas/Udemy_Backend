using FluentValidation;
using Udemy.Auth.Domain.Options;

namespace Udemy.Auth.Application.Validators;

public class PostgresConnectionOptionsValidator : AbstractValidator<PostgresConnectionOptions>
{
    public PostgresConnectionOptionsValidator()
    {
        RuleFor(options => options.Host)
            .NotEmpty().WithMessage("Host is required.")
            .Matches(@"^[a-zA-Z0-9.-]+$").WithMessage("Host must be a valid hostname.");

        RuleFor(options => options.Port)
            .NotEmpty().WithMessage("Port is required.")
            .Matches(@"^\d{4,5}$").WithMessage("Port must be a valid number (4-5 digits).");

        RuleFor(options => options.Database)
            .NotEmpty().WithMessage("Database is required.")
            .Length(3, 100).WithMessage("Database name must be between 3 and 100 characters.");

        RuleFor(options => options.Username)
            .NotEmpty().WithMessage("Username is required.")
            .Length(3, 50).WithMessage("Username must be between 3 and 50 characters.");

        RuleFor(options => options.Password)
            .NotEmpty().WithMessage("Password is required.")
            .Length(8, 100).WithMessage("Password must be at least 8 characters long.");
    }
}