using Microsoft.EntityFrameworkCore;
using Udemy.Common.Base;
using Udemy.Course.Domain.Entities;
using Udemy.Course.Domain.Interfaces;

namespace Udemy.Course.Infrastructure.Repositories;

public class EnrollmentRepository(DbContext context) : BaseRepository<Enrollment>(context), IEnrollmentRepository;