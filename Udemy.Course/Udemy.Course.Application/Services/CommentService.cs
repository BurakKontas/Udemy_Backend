using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Entities;
using Udemy.Course.Domain.Interfaces.Repository;
using Udemy.Course.Domain.Interfaces.Service;

namespace Udemy.Course.Application.Services;

public class CommentService(ICommentRepository commentRepository) : ICommentService
{
    private readonly ICommentRepository _commentRepository = commentRepository;

    public async Task<IEnumerable<Comment>> GetAll(Guid courseId, EndpointFilter filter)
    {
        return await _commentRepository.GetCommentsByCourseIdAsync(courseId, filter);
    }

    public async Task<Comment?> GetByIdAsync(Guid id, Guid courseId)
    {
        return await _commentRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Comment>> GetCommentsByCourseIdAsync(Guid courseId, EndpointFilter filter)
    {
        return await _commentRepository.GetCommentsByCourseIdAsync(courseId, filter);
    }

    public async Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(Guid userId, EndpointFilter filter)
    {
        return await _commentRepository.GetCommentsByUserIdAsync(userId, filter);
    }

    public async Task<Guid> AddAsync(Comment entity)
    {
        return await _commentRepository.AddAsync(entity);
    }

    public async Task<Guid> UpdateAsync(Comment entity, Dictionary<string, object> updates)
    {
        var updated = await _commentRepository.UpdateAsync(entity, updates);

        return updated.Id;
    }

    public async Task<Guid> DeleteAsync(Guid id)
    {
        var comment = await _commentRepository.GetByIdAsync(id);

        if(comment == null)
        {
            throw new Exception("Comment not found");
        }

        await _commentRepository.DeleteAsync(comment);

        return comment.Id;
    }

    public async Task<Comment> GetByIdAsync(Guid id)
    {
        var comment = await _commentRepository.GetByIdAsync(id);

        if(comment == null)
        {
            throw new Exception("Comment not found");
        }

        return comment;
    }
}