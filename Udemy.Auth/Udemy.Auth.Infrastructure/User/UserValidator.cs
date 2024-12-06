using Microsoft.AspNetCore.Identity;

namespace Udemy.Auth.Infrastructure.User;

public class UserValidator : IUserValidator<Domain.User>
{
    public async Task<IdentityResult> ValidateAsync(UserManager<Domain.User> manager, Domain.User user)
    {
        return await Task.FromResult(IdentityResult.Success);
    }
}