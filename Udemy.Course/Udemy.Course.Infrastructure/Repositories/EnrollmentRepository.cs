using Microsoft.EntityFrameworkCore;
using Udemy.Common.Base;
using Udemy.Course.Domain.Entities;
using Udemy.Course.Domain.Interfaces.Repository;
using Udemy.Course.Infrastructure.Contexts;

namespace Udemy.Course.Infrastructure.Repositories;

public class EnrollmentRepository(ApplicationDbContext context) : BaseRepository<Enrollment>(context), IEnrollmentRepository;