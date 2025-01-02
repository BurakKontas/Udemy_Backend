﻿using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Entities;
using Udemy.Course.Domain.Interfaces.Repository;
using Udemy.Course.Domain.Interfaces.Service;

namespace Udemy.Course.Application.Services;

public class FavoriteService(IFavoriteRepository favoriteRepository) : IFavoriteService
{
    private readonly IFavoriteRepository _favoriteRepository = favoriteRepository;

    public async Task<IEnumerable<Favorite>> GetAllAsync(EndpointFilter filter)
    {
        return await _favoriteRepository.GetAll(filter);
    }

    public async Task<Favorite?> GetByIdAsync(Guid id)
    {
        return await _favoriteRepository.GetByIdAsync(id);
    }

    public async Task<Guid> AddAsync(Favorite favorite)
    {
        return await _favoriteRepository.AddAsync(favorite);
    }

    public async Task<Guid> UpdateAsync(Favorite favorite, Dictionary<string, object> updates)
    {
        var updated = await _favoriteRepository.UpdateAsync(favorite, updates);
        return updated.Id;
    }

    public async Task DeleteAsync(Guid id)
    {
        var favorite = await _favoriteRepository.GetByIdAsync(id);

        if (favorite is null)
        {
            throw new KeyNotFoundException();
        }

        await _favoriteRepository.DeleteAsync(favorite);
    }

    public async Task<IEnumerable<Favorite>> GetFavoritesByUserIdAsync(Guid userId, EndpointFilter filter)
    {
        return await _favoriteRepository.GetFavoritesByUserIdAsync(userId, filter);
    }

    public async Task<Guid> AddAsync(Guid userId, Favorite favorite)
    {
        return await _favoriteRepository.AddAsync(userId, favorite);
    }

    public async Task<Guid> DeleteAsync(Guid userId, Guid favoriteId)
    {
        return await _favoriteRepository.DeleteAsync(userId, favoriteId);
    }

    public async Task<bool> IsFavorite(Guid userId, Guid courseId)
    {
        return await _favoriteRepository.IsFavorite(userId, courseId);
    }
}