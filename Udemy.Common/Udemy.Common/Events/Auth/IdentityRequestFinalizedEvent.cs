using System.Net;

namespace Udemy.Common.Events.Auth;

public class IdentityRequestFinalizedEvent
{
    public bool IsSuccess { get; set; } = false;
    public IdentityRequestFailedEvent? Failed { get; set; } = null!;
    public IdentityRequestSucceededEvent? Success { get; set; } = null!;
    public bool IsFailure => !IsSuccess;
};

public record IdentityRequestSucceededEvent(List<string> Roles, bool IsAuthenticated, string? Name, string? AuthenticationType, Guid Id);
public record IdentityRequestFailedEvent(string Reason, HttpStatusCode Code);
