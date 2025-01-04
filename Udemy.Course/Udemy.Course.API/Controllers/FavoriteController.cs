using Asp.Versioning;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Interfaces.Service;

namespace Udemy.Course.API.Controllers;

[Route("/v{version:apiVersion}/favorite")]
[ApiController]
[ApiVersion("1.0")]
public class FavoriteController(IFavoriteService favoriteService) : ControllerBase
{
    private readonly IFavoriteService _favoriteService = favoriteService;

    // get favorite courses
    [Authorize]
    [HttpGet("/get-all")]
    public async Task<IResult> GetAllByUser(UserId userId, EndpointFilter filter)
    {
        var favorites = await _favoriteService.GetFavoritesByUserIdAsync(userId.Value, filter);

        return TypedResults.Ok(favorites);
    }

    // add favorite course
    [Authorize]
    [HttpPost("/add/{courseId:guid}")]
    public async Task<IResult> AddFavorite(Guid courseId, UserId userId)
    {
        var favoriteId = await _favoriteService.AddAsync(userId.Value, courseId);

        return TypedResults.Redirect($"get/{favoriteId}");
    }

    // remove favorite course
    [Authorize]
    [HttpDelete("/delete/{favoriteId:guid}")]
    public async Task<IResult> RemoveFavorite(Guid favoriteId, UserId userId)
    {
        await _favoriteService.DeleteAsync(userId.Value, favoriteId);

        return TypedResults.NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("/delete/{courseId:guid}/user/{userId:guid}")]
    public async Task<IResult> RemoveFavorite(Guid courseId, Guid userId)
    {
        await _favoriteService.DeleteAsync(userId, courseId);

        return TypedResults.NoContent();
    }

    //is favorite
    [Authorize]
    [HttpGet("/is-favorite/{courseId:guid}/user/{userId:guid}")]
    public async Task<IResult> IsFavorite(Guid courseId, Guid userId, UserId consumerId)
    {
        if (userId != consumerId.Value)
            return TypedResults.Unauthorized();

        var isFavorite = await _favoriteService.IsFavorite(userId, courseId);

        return TypedResults.Ok(isFavorite);
    }

    // get favorite details
    [Authorize]
    [HttpGet("/get/{favoriteId:guid}")]
    public async Task<IResult> GetFavorite(Guid favoriteId, UserId userId)
    {
        var favorite = await _favoriteService.GetByIdAsync(userId.Value, favoriteId);

        return TypedResults.Ok(favorite);
    }

}