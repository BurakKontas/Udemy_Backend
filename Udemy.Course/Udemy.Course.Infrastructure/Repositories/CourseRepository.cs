using Microsoft.EntityFrameworkCore;
using Udemy.Common.Base;
using Udemy.Course.Domain.Entities;
using Udemy.Course.Domain.Interfaces.Repository;

namespace Udemy.Course.Infrastructure.Repositories;

public class CourseRepository(DbContext context) : BaseRepository<Domain.Entities.Course>(context), ICourseRepository;