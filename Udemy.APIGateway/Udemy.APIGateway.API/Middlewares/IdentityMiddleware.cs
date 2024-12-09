using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Authentication;
using Udemy.Auth.Contracts.Response;
using Udemy.Common.Consul;

namespace Udemy.APIGateway.API.Middlewares;

public class IdentityMiddleware : IMiddleware
{
    private readonly IConsulDiscoveryService _discoveryService;
    private readonly HttpClient _httpClient;
    private readonly string API_KEY;

    public IdentityMiddleware(HttpClient httpClient, IConsulDiscoveryService discoveryService)
    {
        _discoveryService = discoveryService;

        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        _httpClient = new HttpClient(handler);
        API_KEY = Environment.GetEnvironmentVariable("API_KEY") ?? throw new ArgumentNullException($"env:API_KEY");
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

        var authUri = await _discoveryService.GetServiceUriAsync("Udemy.Auth.API");
        var requestUri = $"{authUri}/identity?token=" + token.Split(" ").Last();

        var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Split(" ").Last());
        request.Headers.Add("X-Api-Key", API_KEY);

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            return;
        }

        var ticket = await response.Content.ReadFromJsonAsync<IdentityResponse>();

        if (ticket == null)
        {
            return;
        }

        context.Request.Headers["X-User-Roles"] = string.Join(",", ticket.Roles);
        context.Request.Headers["X-User-IsAuthenticated"] = ticket.IsAuthenticated.ToString();
        context.Request.Headers["X-User-Name"] = ticket.Name ?? string.Empty;
        context.Request.Headers["X-User-AuthType"] = ticket.AuthenticationType ?? string.Empty;
    }
}