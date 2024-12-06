using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Udemy.Auth.Domain;

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
    public async Task<IActionResult> Register(string email, string password, CancellationToken cancellationToken)
    {
        var user = new User
        {
            UserName = email,
            Email = email,
        };

        user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, password);

        var result = await _userStore.CreateAsync(user, cancellationToken);

        if (result.Succeeded)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var protectedId = await _userStore.GetUserIdAsync(user, cancellationToken);

            await _emailSender.SendConfirmationLinkAsync(user, email, $"https://localhost:5001/api/auth/verify-email?token={token}&id={protectedId}");

            return Ok(new { message = "Registration successful, please check your email to confirm your account." });
        }

        return BadRequest(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(string email, string password, bool useCookie = false)
    {
        var user = await _userManager.FindByNameAsync(email);

        if (user == null)
        {
            return BadRequest("User not found");
        }

        _signInManager.AuthenticationScheme = useCookie ? IdentityConstants.ApplicationScheme : IdentityConstants.BearerScheme;

        var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

        if (!result.Succeeded)
        {
            return Unauthorized(result);
        }

        if (result.Succeeded)
        {
            return Ok(result);
        }
        return BadRequest(result);
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
    public async Task<IActionResult> VerifyEmail(string token, string id, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByIdAsync(id, cancellationToken);

        if(user == null)
        {
            return BadRequest("User not found");
        }

        if (user.EmailConfirmed)
        {
            return BadRequest("Email already confirmed");
        }

        var result = await _userManager.ConfirmEmailAsync(user, token);

        if(result.Succeeded) 
        {
            return Ok("Email confirmed successfully");
        }

        return BadRequest(result);
    }

    [HttpPost("create-role")]
    public async Task<IActionResult> CreateRole(Role role)
    {
        var result = await _roleManager.CreateAsync(role);

        if (result.Succeeded)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
}