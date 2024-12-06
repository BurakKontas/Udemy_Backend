using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Udemy.Auth.Domain;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Udemy.Auth.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(
    IUserStore<User> userStore,
    IRoleStore<Role> roleStore,
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    SignInManager<User> signInManager,
    IEmailSender<User> emailSender,
    IOptionsMonitor<BearerTokenOptions> bearerOptionsMonitor)
    : ControllerBase
{
    private readonly IUserStore<User> _userStore = userStore;
    private readonly IRoleStore<Role> _roleStore = roleStore;
    private readonly RoleManager<Role> _roleManager = roleManager;
    private readonly UserManager<User> _userManager = userManager;
    private readonly SignInManager<User> _signInManager = signInManager;
    private readonly IEmailSender<User> _emailSender = emailSender;
    private readonly BearerTokenOptions _bearerOptions = bearerOptionsMonitor.Get(IdentityConstants.BearerScheme);

    [HttpPost("register")]
    public async Task<Results<ContentHttpResult, BadRequest<IdentityResult>>> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            UserName = request.Email,
            Email = request.Email,
        };

        user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, request.Password);

        var result = await _userStore.CreateAsync(user, cancellationToken);

        if (result.Succeeded)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var protectedId = await _userStore.GetUserIdAsync(user, cancellationToken);

            await _emailSender.SendConfirmationLinkAsync(user, request.Email, $"https://localhost:5001/api/auth/verify-email?token={token}&id={protectedId}");

            return TypedResults.Text("Registration successful, please check your email to confirm your account.");
        }

        return TypedResults.BadRequest(result);
    }

    [HttpPost("login")]
    public async Task<Results<Ok<SignInResult>, UnauthorizedHttpResult, BadRequest<string>>> Login([FromBody] LoginRequest request, bool useCookie = false)
    {
        var user = await _userManager.FindByNameAsync(request.Email);

        if (user == null)
        {
            return TypedResults.BadRequest("User not found");
        }

        _signInManager.AuthenticationScheme = useCookie ? IdentityConstants.ApplicationScheme : IdentityConstants.BearerScheme;

        var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);

        if (!result.Succeeded)
        {
            return TypedResults.Unauthorized();
        }

        return TypedResults.Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<Results<Ok<AccessTokenResponse>, UnauthorizedHttpResult, SignInHttpResult, ChallengeHttpResult>> Refresh(string refreshToken, CancellationToken cancellationToken)
    {
        var refreshTicket = _bearerOptions.RefreshTokenProtector.Unprotect(refreshToken);

        if (refreshTicket?.Properties?.ExpiresUtc is not { } expiresUtc ||
            DateTime.Now >= expiresUtc ||
            await _signInManager.ValidateSecurityStampAsync(refreshTicket.Principal) is not { } user)
        {
            return TypedResults.Challenge();
        }

        var newPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
        return TypedResults.SignIn(newPrincipal, authenticationScheme: IdentityConstants.BearerScheme);
    }

    [HttpGet("verify-email")]
    public async Task<Results<ContentHttpResult, BadRequest<string>>> VerifyEmail(string token, string id, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByIdAsync(id, cancellationToken);

        if (user == null)
        {
            return TypedResults.BadRequest("User not found");
        }

        if (user.EmailConfirmed)
        {
            return TypedResults.BadRequest("Email already confirmed");
        }

        var result = await _userManager.ConfirmEmailAsync(user, token);

        if (result.Succeeded)
        {
            return TypedResults.Text("Email confirmed successfully");
        }

        return TypedResults.BadRequest("Failed to confirm email");
    }

    [HttpPost("create-role")]
    public async Task<Results<ContentHttpResult, BadRequest<IdentityResult>>> CreateRole([FromBody] Role role)
    {
        var result = await _roleManager.CreateAsync(role);

        if (result.Succeeded)
        {
            return TypedResults.Text("Role created.");
        }

        return TypedResults.BadRequest(result);
    }
}

public class RegisterRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}
