using System.Net;
using Consul;
using MassTransit;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading;
using Udemy.Auth.Domain.Interfaces;
using Udemy.Common.Events.Auth;

namespace Udemy.Auth.Application.Handlers;

public class IdentityRequestedEventHandler(IAuthService authService) : IConsumer<IdentityRequestedEvent>
{
    private readonly IAuthService _authService = authService;

    public async Task Consume(ConsumeContext<IdentityRequestedEvent> context)
    {
        var message = context.Message;

        var ticket = _authService.GetIdentityFromToken(message.Token, context.CancellationToken);

        var resultEvent = new IdentityRequestFinalizedEvent();

        if (ticket == null)
        {
            var failed = new IdentityRequestFailedEvent("Token is not valid.", HttpStatusCode.BadRequest);
            resultEvent.Failed = failed;
            await context.RespondAsync(resultEvent);
            return;
        }

        var id = ticket.Properties.Items["Id"];

        if (id == null)
        {
            var failed = new IdentityRequestFailedEvent("Id is not in ticket items.",
                HttpStatusCode.Unauthorized);
            resultEvent.Failed = failed;
            await context.RespondAsync(resultEvent);
            return;
        }

        var identityResponse = new IdentityRequestSucceededEvent(
            ticket.Principal.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList(),
            ticket.Principal.Identity!.IsAuthenticated,
            ticket.Principal.Identity!.Name,
            ticket.Principal.Identity.AuthenticationType,
            Guid.Parse(id!)
        );

        resultEvent.IsSuccess = true;
        resultEvent.Success = identityResponse;

        await context.RespondAsync(resultEvent);
    }
}