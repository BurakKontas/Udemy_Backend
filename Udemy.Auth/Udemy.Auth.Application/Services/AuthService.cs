using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Options;
using Udemy.Auth.Domain.Entities;
using Udemy.Auth.Domain.Interfaces;

namespace Udemy.Auth.Application.Services;

public class AuthService(
    IUserStore<User> userStore,
    IRoleStore<Role> roleStore,
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    SignInManager<User> signInManager,
    IEmailSender<User> emailSender,
    IOptionsMonitor<BearerTokenOptions> bearerOptionsMonitor) : IAuthService
{
    private readonly IUserStore<User> _userStore = userStore;
    private readonly IRoleStore<Role> _roleStore = roleStore;
    private readonly RoleManager<Role> _roleManager = roleManager;
    private readonly UserManager<User> _userManager = userManager;
    private readonly SignInManager<User> _signInManager = signInManager;
    private readonly IEmailSender<User> _emailSender = emailSender;
    private readonly BearerTokenOptions _bearerOptions = bearerOptionsMonitor.Get(IdentityConstants.BearerScheme);

    public async Task<IdentityResult> RegisterUserAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        //check if user already exists
        var existingUser = await _userManager.FindByNameAsync(request.Email);
        if (existingUser != null) return IdentityResult.Failed(new IdentityError { Description = "User already exists.", Code = "USER_EXISTS"});

        var user = new User { UserName = request.Email, Email = request.Email };
        user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, request.Password);

        var result = await _userStore.CreateAsync(user, cancellationToken);
        if (!result.Succeeded) return result;

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var protectedId = await _userStore.GetUserIdAsync(user, cancellationToken);
        await _emailSender.SendConfirmationLinkAsync(user, request.Email, $"https://localhost:5001/api/auth/verify-email?token={token}&id={protectedId}");

        return result;
    }

    public async Task<SignInResult> LoginUserAsync(LoginRequest request, bool useCookie)
    {
        var user = await _userManager.FindByNameAsync(request.Email);
        if (user == null) return SignInResult.Failed;

        _signInManager.AuthenticationScheme = useCookie ? IdentityConstants.ApplicationScheme : IdentityConstants.BearerScheme;
        return await _signInManager.PasswordSignInAsync(user, request.Password, false, false);
    }

    public async Task<bool> ConfirmEmailAsync(string token, string id, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByIdAsync(id, cancellationToken);
        if (user == null || user.EmailConfirmed) return false;

        var result = await _userManager.ConfirmEmailAsync(user, token);
        return result.Succeeded;
    }

    public async Task<string> GeneratePasswordResetTokenAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) throw new Exception("User not found");

        return await _userManager.GeneratePasswordResetTokenAsync(user);
    }

    public async Task<IdentityResult> ResetPasswordAsync(string token, string id, string newPassword, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByIdAsync(id, cancellationToken);
        if (user == null) throw new Exception("User not found");

        return await _userManager.ResetPasswordAsync(user, token, newPassword);
    }

    public async Task<IdentityResult> ChangePasswordAsync(string email, string currentPassword, string newPassword)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) throw new Exception("User not found");

        return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
    }

    public async Task<IdentityResult> CreateRoleAsync(Role role)
    {
        return await _roleManager.CreateAsync(role);
    }

    public async Task<IdentityResult> AddUserToRoleAsync(string email, string roleName)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) throw new Exception("User not found");

        return await _userManager.AddToRoleAsync(user, roleName);
    }

    public async Task<IdentityResult> RemoveUserFromRoleAsync(string email, string roleName)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) throw new Exception("User not found");

        return await _userManager.RemoveFromRoleAsync(user, roleName);
    }

    public async Task<bool> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var refreshTicket = _bearerOptions.RefreshTokenProtector.Unprotect(refreshToken);

        if (refreshTicket?.Properties?.ExpiresUtc is not { } expiresUtc ||
            DateTime.Now >= expiresUtc ||
            await _signInManager.ValidateSecurityStampAsync(refreshTicket.Principal) is not { } user)
        {
            return false;
        }

        var newPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
        return newPrincipal != null;
    }

    public async Task<string> ResendConfirmationEmailAsync(string email, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) throw new Exception("User not found");

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var protectedId = await _userStore.GetUserIdAsync(user, cancellationToken);

        await _emailSender.SendConfirmationLinkAsync(user, email, $"https://localhost:5001/api/auth/verify-email?token={token}&id={protectedId}");
        return "Confirmation email sent.";
    }
}
