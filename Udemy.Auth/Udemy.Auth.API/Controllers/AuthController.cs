﻿using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Udemy.Auth.Domain.Entities;
using Udemy.Auth.Domain.Interfaces;
using Microsoft.AspNetCore.Identity.Data;
using Udemy.Auth.Contracts.Response;

namespace Udemy.Auth.API.Controllers;

[Route("/v{version:apiVersion}")]
[ApiController]
[ApiVersion("1.0")]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("register")]
    public async Task<IResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.RegisterUserAsync(request, cancellationToken);
        if (result.Succeeded)
            return TypedResults.Ok("Registration successful, please check your email to confirm your account.");
        return TypedResults.BadRequest("Registration failed.");
    }

    [HttpPost("login")]
    public async Task<IResult> Login([FromBody] LoginRequest request, bool useCookie = false)
    {
        var result = await _authService.LoginUserAsync(request, useCookie);
        if (!result.Succeeded)
            return TypedResults.Unauthorized();
        return TypedResults.Ok("Login successful.");
    }

    [HttpGet("verify-email")]
    public async Task<IResult> VerifyEmail(string token, string id, CancellationToken cancellationToken)
    {
        var result = await _authService.ConfirmEmailAsync(token, id, cancellationToken);
        if (result.Succeeded)
            return TypedResults.Ok("Email confirmed successfully.");
        return TypedResults.BadRequest("Email confirmation failed.");
    }

    [HttpPost("reset-password")]
    public async Task<IResult> ResetPassword(string token, string id, string newPassword, CancellationToken cancellationToken)
    {
        var result = await _authService.ResetPasswordAsync(token, id, newPassword, cancellationToken);
        if (result.Succeeded)
            return TypedResults.Ok("Password reset successful.");
        return TypedResults.BadRequest("Password reset failed.");
    }

    [HttpPost("change-password")]
    public async Task<IResult> ChangePassword(string email, string currentPassword, string newPassword)
    {
        var result = await _authService.ChangePasswordAsync(email, currentPassword, newPassword);
        if (result.Succeeded)
            return TypedResults.Ok("Password changed successfully.");
        return TypedResults.BadRequest("Password change failed.");
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("create-role")]
    public async Task<IResult> CreateRole([FromBody] Role role)
    {
        var result = await _authService.CreateRoleAsync(role);
        if (result.Succeeded)
            return TypedResults.Ok("Role created successfully.");
        return TypedResults.BadRequest("Role creation failed.");
    }

    //authorize with admin role
    [Authorize(Roles = "Admin")]
    [HttpPost("add-to-role")]
    public async Task<IResult> AddToRole(string email, string roleName)
    {
        var result = await _authService.AddUserToRoleAsync(email, roleName);
        if (result.Succeeded)
            return TypedResults.Ok("User added to role.");
        return TypedResults.BadRequest("Failed to add user to role.");
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("remove-from-role")]
    public async Task<IResult> RemoveFromRole(string email, string roleName)
    {
        var result = await _authService.RemoveUserFromRoleAsync(email, roleName);
        if (result.Succeeded)
            return TypedResults.Ok("User removed from role.");
        return TypedResults.BadRequest("Failed to remove user from role.");
    }

    [HttpGet("resend-confirmation-email")]
    public async Task<IResult> ResendConfirmationEmail(string email, CancellationToken cancellationToken)
    {
        var message = await _authService.ResendConfirmationEmailAsync(email, cancellationToken);
        return TypedResults.Ok(message);
    }
}