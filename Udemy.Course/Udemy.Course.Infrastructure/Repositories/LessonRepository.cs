using Microsoft.EntityFrameworkCore;
using Udemy.Common.Base;
using Udemy.Course.Domain.Entities;
using Udemy.Course.Domain.Interfaces.Repository;

namespace Udemy.Course.Infrastructure.Repositories;

public class LessonRepository(DbContext context) : BaseRepository<Lesson>(context), ILessonRepository;