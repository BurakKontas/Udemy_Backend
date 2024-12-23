﻿using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Udemy.Auth.Domain.Entities;
using Udemy.Auth.Infrastructure.Context;

namespace Udemy.Auth.Infrastructure.User;

public class ProtectedUserStore(
    ApplicationDbContext context,
    IDataProtectionProvider dataProtectionProvider,
    IdentityErrorDescriber describer = null!)
    : UserStore<Domain.Entities.User, Role, ApplicationDbContext>(context, describer), IProtectedUserStore<Domain.Entities.User>
{
    private readonly IDataProtector _dataProtector = dataProtectionProvider.CreateProtector("Identity.PersonalData");

    public override async Task<string> GetUserIdAsync(Domain.Entities.User user, CancellationToken cancellationToken = default)
    {
        return _dataProtector.Protect(await base.GetUserIdAsync(user, cancellationToken));
    }

    public override async Task<Domain.Entities.User?> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var unprotectedId = _dataProtector.Unprotect(userId);
        return await base.FindByIdAsync(unprotectedId, cancellationToken);
    }
}