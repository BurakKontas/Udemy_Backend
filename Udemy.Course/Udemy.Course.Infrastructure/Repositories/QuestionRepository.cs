using Microsoft.EntityFrameworkCore;
using Udemy.Common.Base;
using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Entities;
using Udemy.Course.Domain.Interfaces.Repository;
using Udemy.Course.Infrastructure.Contexts;

namespace Udemy.Course.Infrastructure.Repositories;

public class QuestionRepository(ApplicationDbContext context) : BaseRepository<Question>(context), IQuestionRepository
{
    private readonly ApplicationDbContext _context = context;

    public override async Task<Question?> GetByIdAsync(Guid id)
    {
        var question = await _context.Questions
            .AsNoTracking()
            .Include(x => x.Answers)
            .FirstOrDefaultAsync(x => x.Id == id);

        return question;
    }

    public async Task<IEnumerable<Question>> GetQuestionsByLessonIdAsync(Guid lessonId, EndpointFilter filter)
    {
        var query = _context.Questions
            .AsNoTracking()
            .Include(x => x.Answers)
            .Where(x => x.LessonId == lessonId);

        query = Filtering(query, filter);

        query = Sorting(query, filter);

        query = Paginating(query, filter);

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Question>> GetQuestionsByUserIdAsync(Guid userId, EndpointFilter filter)
    {
        var query = _context.Questions
            .AsNoTracking()
            .Include(x => x.Answers)
            .Where(x => x.UserId == userId);

        query = Filtering(query, filter);

        query = Sorting(query, filter);

        query = Paginating(query, filter);

        return await query.ToListAsync();
    }
}