using Microsoft.EntityFrameworkCore;
using Udemy.Common.Base;
using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Entities;
using Udemy.Course.Domain.Interfaces.Repository;
using Udemy.Course.Infrastructure.Contexts;

namespace Udemy.Course.Infrastructure.Repositories;

public class AnswerRepository(ApplicationDbContext context) : BaseRepository<Answer>(context), IAnswerRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<Answer>> GetAnswersByQuestionIdAsync(Guid questionId, EndpointFilter filter)
    {
        var query = _context.Answers
            .AsNoTracking()
            .Where(x => x.QuestionId == questionId)
            .AsQueryable();

        query = Filtering(query, filter);

        query = Sorting(query, filter);

        query = Paginating(query, filter);

        return await query.ToListAsync();
        
    }

    public async Task<IEnumerable<Answer>> GetAnswersByUserIdAsync(Guid userId, EndpointFilter filter)
    {
        var query = _context.Answers
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .AsQueryable();

        query = Filtering(query, filter);

        query = Sorting(query, filter);

        query = Paginating(query, filter);

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Answer>> GetAnswersByQuestionIdAndUserIdAsync(Guid questionId, Guid userId, EndpointFilter filter)
    {
        var query = _context.Answers
            .AsNoTracking()
            .Where(x => x.QuestionId == questionId && x.UserId == userId)
            .AsQueryable();

        query = Filtering(query, filter);

        query = Sorting(query, filter);

        query = Paginating(query, filter);

        return await query.ToListAsync();
    }

    public async Task<Guid> AddAsync(Guid questionId, Answer answer)
    {
        answer.QuestionId = questionId;

        await _context.Answers.AddAsync(answer);

        await _context.SaveChangesAsync();

        return answer.Id;
    }

    public async Task DeleteAsync(Guid questionId, Guid answerId)
    {
        var answer = await _context.Answers
            .FirstOrDefaultAsync(x => x.QuestionId == questionId && x.Id == answerId);

        if (answer is null)
        {
            throw new KeyNotFoundException();
        }

        _context.Answers.Remove(answer);

        await _context.SaveChangesAsync();
    }
}