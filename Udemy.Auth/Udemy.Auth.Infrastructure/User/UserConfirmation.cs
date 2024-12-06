using Microsoft.AspNetCore.Identity;

namespace Udemy.Auth.Infrastructure.User;

public class UserConfirmation : IUserConfirmation<Domain.User>
{
    public async Task<bool> IsConfirmedAsync(UserManager<Domain.User> manager, Domain.User user)
    {
        return await Task.FromResult(user.EmailConfirmed);
    }
}