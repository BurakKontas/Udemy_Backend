using System.Security.Principal;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Udemy.Auth.Domain.Entities;

namespace Udemy.Auth.Domain.Interfaces;

public interface IAuthService
{
    Task<IdentityResult> RegisterUserAsync(RegisterRequest request, CancellationToken cancellationToken);
    Task<SignInResult> LoginUserAsync(LoginRequest request, bool useCookie);
    Task<IdentityResult> ConfirmEmailAsync(string token, string id, CancellationToken cancellationToken);
    Task<string> GeneratePasswordResetTokenAsync(string email);
    Task<IdentityResult> ResetPasswordAsync(string token, string id, string newPassword, CancellationToken cancellationToken);
    Task<IdentityResult> ChangePasswordAsync(string email, string currentPassword, string newPassword);
    Task<IdentityResult> CreateRoleAsync(Role role);
    Task<IdentityResult> AddUserToRoleAsync(string email, string roleName);
    Task<IdentityResult> RemoveUserFromRoleAsync(string email, string roleName);
    Task<bool> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
    Task<string> ResendConfirmationEmailAsync(string email, CancellationToken cancellationToken);
    public AuthenticationTicket GetIdentityFromToken(string token, CancellationToken cancellationToken);
    public string[] GetRolesFromTicket(AuthenticationTicket ticket, CancellationToken cancellationToken);
}