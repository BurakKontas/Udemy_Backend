using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication;
using Udemy.Auth.Domain.Entities;
using Udemy.Auth.Domain.Interfaces;

namespace Udemy.Auth.Application.Services;

public class AuthService : IAuthService
{
    private readonly RoleManager<Role> _roleManager;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IEmailSender<User> _emailSender;
    private readonly BearerTokenOptions _bearerOptions;

    public AuthService(UserManager<User> userManager,
        RoleManager<Role> roleManager,
        SignInManager<User> signInManager,
        IEmailSender<User> emailSender,
        IOptionsMonitor<BearerTokenOptions> bearerOptionsMonitor)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
        _bearerOptions = bearerOptionsMonitor.Get(IdentityConstants.BearerScheme);

        // remove default user validator because it causes issues with data protectors
        var defaultValidator = userManager.UserValidators.First(v => v is Microsoft.AspNetCore.Identity.UserValidator<User>);
        userManager.UserValidators.Remove(defaultValidator);
    }

    public async Task<IdentityResult> RegisterUserAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        //check if user already exists
        var existingUser = await _userManager.FindByNameAsync(request.Email);
        if (existingUser != null)
        {
            await SendConfirmationEmail(existingUser, request.Email, cancellationToken);
            return IdentityResult.Success;
        }

        var user = new User { UserName = request.Email, Email = request.Email };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded) return result;
        

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var protectedId = await _userManager.GetUserIdAsync(user);
        await _emailSender.SendConfirmationLinkAsync(user, request.Email, $"http://localhost:3000/api/auth/verify-email?token={token}&id={protectedId}");

        return result;
    }

    private async Task SendConfirmationEmail(User user,string email, CancellationToken cancellationToken)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var protectedId = await _userManager.GetUserIdAsync(user);
        await _emailSender.SendConfirmationLinkAsync(user, email, $"http://localhost:3000/api/auth/verify-email?token={token}&id={protectedId}");
    }

    public async Task<SignInResult> LoginUserAsync(LoginRequest request, bool useCookie)
    {
        var user = await _userManager.FindByNameAsync(request.Email);
        if (user == null) throw new ArgumentException($"User not found.");

        _signInManager.AuthenticationScheme = useCookie ? IdentityConstants.ApplicationScheme : IdentityConstants.BearerScheme;
        return await _signInManager.PasswordSignInAsync(user, request.Password, false, false);
    }

    public async Task<IdentityResult> ConfirmEmailAsync(string token, string id, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found.", Code = "USER_NOT_FOUND"});

        var result = await _userManager.ConfirmEmailAsync(user, token);
        return result;

    }

    public async Task<string> GeneratePasswordResetTokenAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) throw new ArgumentNullException($"User not found");

        return await _userManager.GeneratePasswordResetTokenAsync(user);
    }

    public async Task<IdentityResult> ResetPasswordAsync(string token, string id, string newPassword, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) throw new ArgumentException($"User not found");

        return await _userManager.ResetPasswordAsync(user, token, newPassword);
    }

    public async Task<IdentityResult> ChangePasswordAsync(string email, string currentPassword, string newPassword)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) throw new ArgumentException($"User not found");

        return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
    }

    public async Task<IdentityResult> CreateRoleAsync(Role role)
    {
        return await _roleManager.CreateAsync(role);
    }

    public async Task<IdentityResult> AddUserToRoleAsync(string email, string roleName)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) throw new ArgumentException($"User not found");

        // check if role exists
        if (await _roleManager.FindByNameAsync(roleName) == null) throw new ArgumentNullException($"Role not found");

        return await _userManager.AddToRolesAsync(user, [roleName]);
    }

    public async Task<IdentityResult> RemoveUserFromRoleAsync(string email, string roleName)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) throw new ArgumentException($"User not found");

        return await _userManager.RemoveFromRoleAsync(user, roleName);
    }

    public async Task<bool> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var refreshTicket = _bearerOptions.RefreshTokenProtector.Unprotect(refreshToken);
        var timeProvider = _bearerOptions.TimeProvider;

        if (refreshTicket?.Properties?.ExpiresUtc is not { } expiresUtc ||
            timeProvider!.GetUtcNow() >= expiresUtc ||
            await _signInManager.ValidateSecurityStampAsync(refreshTicket.Principal) is not { } user)
        {
            return false;
        }

        var newPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
        return newPrincipal != null!;
    }

    public async Task<string> ResendConfirmationEmailAsync(string email, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) throw new ArgumentNullException($"User not found");

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var protectedId = await _userManager.GetUserIdAsync(user);

        await _emailSender.SendConfirmationLinkAsync(user, email, $"https://localhost:5001/api/auth/verify-email?token={token}&id={protectedId}");
        return "Confirmation email sent.";
    }

    public AuthenticationTicket GetIdentityFromToken(string token, CancellationToken cancellationToken)
    {
        var unprotectedToken = _bearerOptions.BearerTokenProtector.Unprotect(token);
        if (unprotectedToken == null) throw new ArgumentNullException($"Invalid token");

        return unprotectedToken;
    }

    public string[] GetRolesFromTicket(AuthenticationTicket ticket, CancellationToken cancellationToken)
    {
        var roles = ticket.Principal.Claims.Where(c => c.Type == ticket.Principal.Identities.First().RoleClaimType).Select(c => c.Value);
        return roles.ToArray();
    }
}
