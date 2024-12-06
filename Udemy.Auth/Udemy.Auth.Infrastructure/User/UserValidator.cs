using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;

namespace Udemy.Auth.Infrastructure.User;

public partial class UserValidator : IUserValidator<Domain.Entities.User>
{
    [GeneratedRegex("""(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))""")]
    private static partial Regex EmailRegex();

    public async Task<IdentityResult> ValidateAsync(UserManager<Domain.Entities.User> manager, Domain.Entities.User user)
    {
        var errors = new List<IdentityError>();

        // Check if email is valid
        if (string.IsNullOrWhiteSpace(user.Email) || !IsValidEmail(user.Email))
        {
            errors.Add(new IdentityError
            {
                Code = "InvalidEmail",
                Description = "Email is not valid."
            });
        }

        // Check if email is unique
        var existingUser = await manager.FindByEmailAsync(user.Email!);
        if (existingUser != null && existingUser.Id != user.Id) {
            errors.Add(new IdentityError
            {
                Code = "DuplicateEmail",
                Description = "Email is already taken."
            });
        }

        // Check if username is unique
        if (existingUser != null && existingUser.Id != user.Id)
        {
            errors.Add(new IdentityError
            {
                Code = "DuplicateUserName",
                Description = "Username is already taken."
            });
        }

        return errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed([..errors]);
    }

    private bool IsValidEmail(string email)
    {
        var regex = EmailRegex();
        return regex.IsMatch(email);
    }
}