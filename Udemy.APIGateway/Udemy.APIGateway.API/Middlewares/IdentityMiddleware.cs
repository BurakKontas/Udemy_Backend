using MassTransit;
using Udemy.Common.Consul;
using Udemy.Common.Events.Auth;
using Udemy.Common.Helpers;

namespace Udemy.APIGateway.API.Middlewares;

public class IdentityMiddleware : IMiddleware
{
    private readonly IConsulDiscoveryService _discoveryService;
    private readonly HttpClient _httpClient;
    private readonly string API_KEY;
    private readonly string SALT;
    private readonly IBus _bus;

    public IdentityMiddleware(HttpClient httpClient, IConsulDiscoveryService discoveryService, IBus bus)
    {
        _discoveryService = discoveryService;

        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        _httpClient = new HttpClient(handler);
        API_KEY = Environment.GetEnvironmentVariable("API_KEY") ?? throw new ArgumentNullException($"env:API_KEY");
        SALT = Environment.GetEnvironmentVariable("SALT") ?? throw new ArgumentNullException($"env:SALT");
        _bus = bus;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        await AddAuthenticationTicketToHeaders(context);
        await next(context);
    }

    private async Task AddAuthenticationTicketToHeaders(HttpContext context)
    {
        context.Request.Headers["X-Api-Key"] = API_KEY;

        var token = context.Request.Headers.Authorization.ToString();

        if (string.IsNullOrEmpty(token))
        {
            return;
        }

        var identityEvent = new IdentityRequestedEvent(token);

        var response = await _bus.Request<IdentityRequestedEvent, IdentityRequestFinalizedEvent>(identityEvent);

        if(response.Message.IsFailure) 
        {
            return;
        }

        var ticket = response.Message.Success;

        if(ticket == null)
        {
            throw new InvalidOperationException("Ticket is null.");
        }

        context.Request.Headers["X-User-Roles"] = string.Join(",", ticket!.Roles);
        context.Request.Headers["X-User-IsAuthenticated"] = ticket.IsAuthenticated.ToString();
        context.Request.Headers["X-User-Name"] = ticket.Name ?? string.Empty;
        context.Request.Headers["X-User-AuthType"] = ticket.AuthenticationType ?? string.Empty;
        context.Request.Headers["X-User-Id"] = ticket.Id.ToString();

        var secret = context.Request.Headers["X-User-Roles"] + context.Request.Headers["X-User-IsAuthenticated"] + context.Request.Headers["X-User-Name"] + context.Request.Headers["X-User-AuthType"] + context.Request.Headers["X-User-Id"];
        secret = Sha256Helper.Hash(secret, SALT);

        context.Request.Headers["X-User-Secret"] = secret;
    }
}