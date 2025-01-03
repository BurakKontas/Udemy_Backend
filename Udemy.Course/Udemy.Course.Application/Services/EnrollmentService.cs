using System.Linq.Expressions;
using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Entities;
using Udemy.Course.Domain.Interfaces.Repository;
using Udemy.Course.Domain.Interfaces.Service;

namespace Udemy.Course.Application.Services;

public class EnrollmentService(IEnrollmentRepository enrollmentRepository) : IEnrollmentService
{
    private readonly IEnrollmentRepository _enrollmentRepository = enrollmentRepository;

    public async Task<Guid> AddAsync(Guid userId, Guid courseId)
    {
        var enrollment = Enrollment.Create(userId, courseId);
        return await _enrollmentRepository.AddAsync(enrollment, courseId);
    }

    public async Task<IEnumerable<Enrollment>> GetAll(Guid courseId, EndpointFilter filter)
    {
        return await _enrollmentRepository.GetAll(courseId, filter);
    }

    public async Task<Enrollment?> GetByIdAsync(Guid id, Guid courseId)
    {
        return await _enrollmentRepository.GetByIdAsync(id, courseId);
    }

    public async Task<IEnumerable<Enrollment>> GetManyAsync(Expression<Func<Enrollment, bool>> predicate, Guid courseId, EndpointFilter filter)
    {
        return await _enrollmentRepository.GetManyAsync(predicate, courseId, filter);
    }

    public async Task<Guid> UpdateAsync(Guid id, Dictionary<string, object> updates)
    {
        var enrollment = await _enrollmentRepository.GetByIdAsync(id);

        if (enrollment == null)
        {
            throw new Exception("Enrollment not found");
        }

        await _enrollmentRepository.UpdateAsync(enrollment, updates);

        return id;
    }

    public async Task<Guid> DeleteAsync(Guid id)
    {
        var enrollment = await _enrollmentRepository.GetByIdAsync(id);

        if (enrollment == null)
        {
            throw new Exception("Enrollment not found");
        }

        await _enrollmentRepository.DeleteAsync(enrollment);

        return id;
    }

    public async Task<IEnumerable<Enrollment>> GetAllAsync(EndpointFilter filter)
    {
        return await _enrollmentRepository.GetAll(filter);
    }

    public async Task<Enrollment?> GetByIdAsync(Guid id)
    {
        return await _enrollmentRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Enrollment>> GetManyAsync(Expression<Func<Enrollment, bool>> predicate, EndpointFilter filter)
    {
        return await _enrollmentRepository.GetManyAsync(predicate, filter);
    }

    public async Task<Guid> AddAsync(Enrollment entity, Guid courseId)
    {
        return await _enrollmentRepository.AddAsync(entity, courseId);
    }

    public async Task<IEnumerable<Enrollment>> GetAllAsync(Guid courseId, EndpointFilter filter)
    {
        return await _enrollmentRepository.GetAllByCourseId(courseId, filter);
    }

    public async Task<IEnumerable<Enrollment>> GetAllByUserAsync(Guid userId, EndpointFilter filter)
    {
        return await _enrollmentRepository.GetAllByUserId(userId, filter);
    }
}