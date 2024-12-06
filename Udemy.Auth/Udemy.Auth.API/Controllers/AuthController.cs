using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.Data;
using Udemy.Auth.Domain.Entities;
using Udemy.Auth.Domain.Interfaces;

namespace Udemy.Auth.API.Controllers;

[Route("/")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.RegisterUserAsync(request, cancellationToken);
        if (result.Succeeded) return Ok("Registration successful, please check your email to confirm your account.");
        return BadRequest(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, bool useCookie = false)
    {
        var result = await _authService.LoginUserAsync(request, useCookie);
        if (!result.Succeeded) return Unauthorized();
        return Ok("Login successful.");
    }

    [HttpGet("verify-email")]
    public async Task<IActionResult> VerifyEmail(string token, string id, CancellationToken cancellationToken)
    {
        var success = await _authService.ConfirmEmailAsync(token, id, cancellationToken);
        if (success) return Ok("Email confirmed successfully.");
        return BadRequest("Email confirmation failed.");
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(string token, string id, string newPassword, CancellationToken cancellationToken)
    {
        var result = await _authService.ResetPasswordAsync(token, id, newPassword, cancellationToken);
        if (result.Succeeded) return Ok("Password reset successful.");
        return BadRequest("Password reset failed.");
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(string email, string currentPassword, string newPassword)
    {
        var result = await _authService.ChangePasswordAsync(email, currentPassword, newPassword);
        if (result.Succeeded) return Ok("Password changed successfully.");
        return BadRequest("Password change failed.");
    }

    [HttpPost("create-role")]
    public async Task<IActionResult> CreateRole([FromBody] Role role)
    {
        var result = await _authService.CreateRoleAsync(role);
        if (result.Succeeded) return Ok("Role created successfully.");
        return BadRequest("Role creation failed.");
    }

    [HttpPost("add-to-role")]
    public async Task<IActionResult> AddToRole(string email, string roleName)
    {
        var result = await _authService.AddUserToRoleAsync(email, roleName);
        if (result.Succeeded) return Ok("User added to role.");
        return BadRequest("Failed to add user to role.");
    }

    [HttpPost("remove-from-role")]
    public async Task<IActionResult> RemoveFromRole(string email, string roleName)
    {
        var result = await _authService.RemoveUserFromRoleAsync(email, roleName);
        if (result.Succeeded) return Ok("User removed from role.");
        return BadRequest("Failed to remove user from role.");
    }

    [HttpGet("resend-confirmation-email")]
    public async Task<IActionResult> ResendConfirmationEmail(string email, CancellationToken cancellationToken)
    {
        var message = await _authService.ResendConfirmationEmailAsync(email, cancellationToken);
        return Ok(message);
    }
}