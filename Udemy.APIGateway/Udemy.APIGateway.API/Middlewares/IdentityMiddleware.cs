using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using Udemy.Auth.Contracts.Response;

namespace Udemy.APIGateway.API.Middlewares;

public class IdentityMiddleware(HttpClient httpClient) : IMiddleware
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        await GetAndAddAuthenticationTicket(context);
        await next(context);
    }

    private async Task GetAndAddAuthenticationTicket(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        if (string.IsNullOrEmpty(token))
        {
            throw new AuthenticationFailureException("Authorization token is missing.");
        }

        var request = new HttpRequestMessage(HttpMethod.Get, "/identity");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            throw new AuthenticationFailureException("Invalid token.");
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        var ticket = JsonSerializer.Deserialize<AuthenticationTicket>(responseContent);
        if (ticket == null)
            throw new AuthenticationFailureException("AuthenticationTicket is null");

        context.Items["AuthenticationTicket"] = ticket;
    }
}