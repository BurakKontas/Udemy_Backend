using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Mail;
using Udemy.Auth.Domain.Options;
using Udemy.Auth.Infrastructure.Context;
using Udemy.Auth.Infrastructure.DataProtection;
using Udemy.Auth.Infrastructure.Repositories;
using Udemy.Auth.Infrastructure.User;
using Udemy.Common.Consul;
using DataProtectionProvider = Udemy.Auth.Infrastructure.DataProtection.DataProtectionProvider;
using Role = Udemy.Auth.Domain.Entities.Role;

namespace Udemy.Auth.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthorization();
        services.AddAuthentication()
            .AddCookie(IdentityConstants.ApplicationScheme, options =>
            {
                options.Cookie.Expiration = TimeSpan.FromDays(150);
            })
            .AddBearerToken(IdentityConstants.BearerScheme, options =>
            {
                options.BearerTokenExpiration = TimeSpan.FromMinutes(15);
                options.RefreshTokenExpiration = TimeSpan.FromDays(30);
            });

        services.AddDataProtection()
            .PersistKeysToDbContext<ApplicationDbContext>()
            .SetApplicationName("Udemy")
            .SetDefaultKeyLifetime(TimeSpan.FromDays(90));

        services.AddScoped<IPersonalDataProtector, PersonalDataProtector>();
        services.AddScoped<ILookupProtectorKeyRing, KeyRing>();
        services.AddScoped<ILookupProtector, LookupProtector>();
        services.AddIdentityCore<Domain.Entities.User>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedAccount = true;

                options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultProvider;
                options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;
                options.Tokens.ChangeEmailTokenProvider = TokenOptions.DefaultProvider;
                options.Tokens.ChangePhoneNumberTokenProvider = TokenOptions.DefaultProvider;
                options.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultProvider;
                options.Tokens.AuthenticatorIssuer = "Udemy.Auth";

                options.Stores.ProtectPersonalData = true;
            })
            .AddUserConfirmation<UserConfirmation>()
            .AddUserValidator<User.UserValidator<Domain.Entities.User>>()
            .AddRoles<Role>()
            .AddRoleManager<RoleManager<Role>>()
            .AddSignInManager<SignInManager<Domain.Entities.User>>()
            .AddUserManager<UserManager<Domain.Entities.User>>()
            .AddRoleStore<RoleStore<Role, ApplicationDbContext>>()
            .AddUserStore<ProtectedUserStore>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddTokenProvider<DataProtectionProvider>(TokenOptions.DefaultProvider);
            //.AddApiEndpoints();

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            var connectionString = PostgresConnectionOptions.FromEnvironment()
                .BuildConnectionString();
            options.UseNpgsql(connectionString);
        });

        var resendApiKey = configuration.GetValue("resend:apikey", "");
        services.AddSingleton(_ => new SmtpClient
        {
            Host = "smtp.resend.com",
            Port = 587,
            Credentials = new NetworkCredential("resend", resendApiKey),
            EnableSsl = true
        });
        services.AddScoped<IEmailSender<Domain.Entities.User>, SmtpEmailSender>();

        services.AddConsul(configuration);

        return services;
    }
}