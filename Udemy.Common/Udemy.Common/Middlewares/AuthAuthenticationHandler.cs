using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Principal;
using System.Text.Encodings.Web;
using Udemy.Common.Helpers;

namespace Udemy.Common.Middlewares;

public class AuthAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{

    private readonly string API_KEY =
        Environment.GetEnvironmentVariable("API_KEY") ?? throw new ArgumentNullException($"env:API_KEY");

    private readonly string SALT =
        Environment.GetEnvironmentVariable("SALT") ?? throw new ArgumentNullException($"env:SALT");

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var receivedApiKey = Context.Request.Headers["X-Api-Key"].FirstOrDefault();

        if (string.IsNullOrEmpty(receivedApiKey) || !receivedApiKey.Equals(API_KEY))
        {
            return AuthenticateResult.Fail("API_KEY is not valid.");
        }

        var roles = Context.Request.Headers["X-User-Roles"].FirstOrDefault()?.Split(',');
        var isAuthenticated = Context.Request.Headers["X-User-IsAuthenticated"].FirstOrDefault();
        var name = Context.Request.Headers["X-User-Name"].FirstOrDefault();
        var authType = Context.Request.Headers["X-User-AuthType"].FirstOrDefault();
        var secret = Context.Request.Headers["X-User-Secret"].FirstOrDefault();
        var userId = Guid.Parse(Context.Request.Headers["X-User-Id"].FirstOrDefault()!);

        if(secret == null)
        {
            return AuthenticateResult.Fail("Missing secret header.");
        }

        var secretText = Context.Request.Headers["X-User-Roles"] + Context.Request.Headers["X-User-IsAuthenticated"] + Context.Request.Headers["X-User-Name"] + Context.Request.Headers["X-User-AuthType"] + Context.Request.Headers["X-User-Id"];

        var isSecretVerified = Sha256Helper.Verify(secretText, SALT, secret);

        if (!isSecretVerified)
        {
            return AuthenticateResult.Fail("Secret is not valid.");
        }

        if (roles == null || isAuthenticated == null || name == null || authType == null)
        {
            return AuthenticateResult.Fail("Missing authentication headers.");
        }

        var identity = new GenericIdentity(name, authType);
        var principal = new GenericPrincipal(identity, roles);

        Context.User = principal;

        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        ticket.Properties.Items["Id"] = userId.ToString();

        return AuthenticateResult.Success(ticket);
    }
}