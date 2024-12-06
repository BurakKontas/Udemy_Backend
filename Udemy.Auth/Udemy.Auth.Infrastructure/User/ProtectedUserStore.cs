using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Udemy.Auth.Domain;
using Udemy.Auth.Infrastructure.Context;

namespace Udemy.Auth.Infrastructure.User;

public class ProtectedUserStore(
    ApplicationDbContext context,
    IDataProtectionProvider dataProtectionProvider,
    IdentityErrorDescriber describer = null!)
    : UserStore<Domain.User, Role, ApplicationDbContext>(context, describer), IProtectedUserStore<Domain.User>
{
    private readonly IDataProtector _dataProtector = dataProtectionProvider.CreateProtector("Identity.PersonalData");

    public override async Task<string> GetUserIdAsync(Domain.User user, CancellationToken cancellationToken = default)
    {
        return _dataProtector.Protect(await base.GetUserIdAsync(user, cancellationToken));
    }

    public override async Task<Domain.User?> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var unprotectedId = _dataProtector.Unprotect(userId);
        return await base.FindByIdAsync(unprotectedId, cancellationToken);
    }
}