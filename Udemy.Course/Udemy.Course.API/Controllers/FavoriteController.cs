using Asp.Versioning;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Interfaces.Service;

namespace Udemy.Course.API.Controllers;

[Route("/favorite/v{version:apiVersion}")]
[ApiController]
[ApiVersion("1.0")]
public class FavoriteController(IFavoriteService favoriteService) : ControllerBase
{
    private readonly IFavoriteService _favoriteService = favoriteService;

    // get favorite courses
    [HttpGet("/get-all/{userId:guid}")]
    public async Task<IResult> GetAllByUser(Guid userId, EndpointFilter filter)
    {
        var favorites = await _favoriteService.GetFavoritesByUserIdAsync(userId, filter);

        return TypedResults.Ok(favorites);
    }

    // add favorite course
    [HttpPost("/add/{courseId:guid}/user/{userId:guid}")]
    public async Task<IResult> AddFavorite(Guid courseId, Guid userId)
    {
        var favoriteId = await _favoriteService.AddAsync(userId, courseId);

        return TypedResults.Redirect($"get/{favoriteId}");
    }

    // remove favorite course
    [HttpDelete("/delete/{favoriteId:guid}")]
    public async Task<IResult> RemoveFavorite(Guid favoriteId)
    {
        await _favoriteService.DeleteAsync(favoriteId);

        return TypedResults.NoContent();
    }

    [HttpDelete("/delete/{courseId:guid}/user/{userId:guid}")]
    public async Task<IResult> RemoveFavorite(Guid courseId, Guid userId)
    {
        await _favoriteService.DeleteAsync(userId, courseId);

        return TypedResults.NoContent();
    }

    //is favorite
    [HttpGet("/is-favorite/{courseId:guid}/user/{userId:guid}")]
    public async Task<IResult> IsFavorite(Guid courseId, Guid userId)
    {
        var isFavorite = await _favoriteService.IsFavorite(userId, courseId);

        return TypedResults.Ok(isFavorite);
    }

    // get favorite details
    [HttpGet("/get/{favoriteId:guid}")]
    public async Task<IResult> GetFavorite(Guid favoriteId)
    {
        var favorite = await _favoriteService.GetByIdAsync(favoriteId);

        return TypedResults.Ok(favorite);
    }

}