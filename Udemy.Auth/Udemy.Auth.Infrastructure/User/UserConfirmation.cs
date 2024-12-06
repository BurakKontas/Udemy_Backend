using Microsoft.AspNetCore.Identity;
using Udemy.Auth.Domain.Interfaces;

namespace Udemy.Auth.Infrastructure.User;

public class UserConfirmation : IUserConfirmation<Domain.Entities.User>
{
    public async Task<bool> IsConfirmedAsync(UserManager<Domain.Entities.User> manager, Domain.Entities.User user)
    {
        if (!user.EmailConfirmed)
            return await Task.FromResult(false);

        if (await manager.IsLockedOutAsync(user))
            return false;

        if (user is IDeactivatable { IsDeactivated: true })
            return false;
        
        return true;
    }
}