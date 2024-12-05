namespace Udemy.Auth.Infrastructure;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Udemy.Auth.Domain;
using Microsoft.AspNetCore.DataProtection;

public class ProtectedUserStore(
    ApplicationDbContext context,
    IDataProtectionProvider dataProtectionProvider,
    IdentityErrorDescriber describer = null!)
    : UserStore<User, Role, ApplicationDbContext>(context, describer)
{
    private readonly IDataProtector _dataProtector = dataProtectionProvider.CreateProtector("Identity.PersonalData");

    public override async Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken = default)
    {
        return _dataProtector.Protect(await base.GetUserIdAsync(user, cancellationToken));
    }

    public override async Task<User?> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var unprotectedId = _dataProtector.Unprotect(userId);
        return await base.FindByIdAsync(unprotectedId, cancellationToken);
    }
}