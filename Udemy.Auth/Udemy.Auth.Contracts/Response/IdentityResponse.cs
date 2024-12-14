namespace Udemy.Auth.Contracts.Response;

public record IdentityResponse(IEnumerable<string> Roles, bool IsAuthenticated, string? Name, string? AuthenticationType, Guid Id);