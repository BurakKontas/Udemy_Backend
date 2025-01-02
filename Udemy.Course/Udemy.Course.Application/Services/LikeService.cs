using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Entities;
using Udemy.Course.Domain.Interfaces.Repository;
using Udemy.Course.Domain.Interfaces.Service;

namespace Udemy.Course.Application.Services;

public class LikeService(ILikeRepository likeRepository) : ILikeService
{
    private readonly ILikeRepository _likeRepository = likeRepository;

    public async Task<IEnumerable<Like>> GetLikesByAnswerIdAsync(Guid answerId, EndpointFilter filter)
    {
        return await _likeRepository.GetLikesByAnswerIdAsync(answerId, filter);
    }

    public async Task<IEnumerable<Like>> GetLikesByUserIdAsync(Guid userId, EndpointFilter filter)
    {
        return await _likeRepository.GetLikesByUserIdAsync(userId, filter);
    }

    public async Task<Guid> AddAsync(Guid answerId, Like like)
    {
        return await _likeRepository.AddAsync(answerId, like);
    }

    public async Task DeleteAsync(Guid answerId, Guid likeId)
    {
        await _likeRepository.DeleteAsync(answerId, likeId);
    }

    public async Task<bool> IsLiked(Guid userId, Guid answerId)
    {
        return await _likeRepository.IsLiked(userId, answerId);
    }

    public async Task<Like> GetLikeByIdAsync(Guid userId, Guid likeId)
    {
        return await _likeRepository.GetLikeByIdAsync(userId, likeId);
    }

    public async Task<IEnumerable<Like>> GetAllAsync(EndpointFilter filter)
    {
        return await _likeRepository.GetAll(filter);
    }

    public async Task<Like> GetByIdAsync(Guid id)
    {

        var like = await _likeRepository.GetByIdAsync(id);

        if (like is null)
        {
            throw new KeyNotFoundException();
        }

        return like;
    }

    public async Task<Guid> UpdateAsync(Like like, Dictionary<string, object> updates)
    {
        var updated = await _likeRepository.UpdateAsync(like, updates);
        return updated.Id;
    }

    public async Task<Guid> DeleteAsync(Guid id)
    {
        var like = await _likeRepository.GetByIdAsync(id);

        if (like is null)
        {
            throw new KeyNotFoundException();
        }

        await _likeRepository.DeleteAsync(like);
        return like.Id;
    }
}