using Microsoft.AspNetCore.Identity;
using Udemy.Auth.Domain;

namespace Udemy.Auth.Infrastructure;

public class UserValidator : IUserValidator<User>
{
    public async Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user)
    {
        return await Task.FromResult(IdentityResult.Success);
    }
}