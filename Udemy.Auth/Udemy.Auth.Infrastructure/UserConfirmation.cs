using Microsoft.AspNetCore.Identity;
using Udemy.Auth.Domain;

namespace Udemy.Auth.Infrastructure;

public class UserConfirmation : IUserConfirmation<User>
{
    public async Task<bool> IsConfirmedAsync(UserManager<User> manager, User user)
    {
        return await Task.FromResult(user.EmailConfirmed);
    }
}