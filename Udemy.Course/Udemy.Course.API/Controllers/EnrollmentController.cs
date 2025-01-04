using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Interfaces.Service;

namespace Udemy.Course.API.Controllers;

[Route("/v{version:apiVersion}/enrollment")]
[ApiController]
[ApiVersion("1.0")]
public class EnrollmentController(IEnrollmentService enrollmentService) : ControllerBase
{
    private readonly IEnrollmentService _enrollmentService = enrollmentService;

    // enroll
    [Authorize]
    [HttpPost("/enroll/{courseId:guid}")]
    public async Task<IResult> Enroll(Guid courseId, UserId userId)
    {
        var enrollmentId = await _enrollmentService.AddAsync(userId.Value, courseId);

        return TypedResults.Redirect($"get/{enrollmentId}");
    }

    // remove enrollment
    [Authorize(Roles = "Admin")]
    [HttpDelete("/delete/{enrollmentId:guid}")]
    public async Task<IResult> RemoveEnrollment(Guid enrollmentId)
    {
        await _enrollmentService.DeleteAsync(enrollmentId);

        return TypedResults.NoContent();
    }

    // get enrollment details
    [Authorize]
    [HttpGet("/details/{enrollmentId:guid}")]
    public async Task<IResult> GetEnrollment(Guid enrollmentId, UserId userId)
    {
        var enrollment = await _enrollmentService.GetByIdAsync(userId.Value, enrollmentId);

        return TypedResults.Ok(enrollment);
    }

    // get course enrollments
    [Authorize]
    [HttpGet("/enrollments/{courseId:guid}")]
    public async Task<IResult> GetEnrollmentCount(Guid courseId, EndpointFilter filter, UserId userId)
    {
        var enrollments = await _enrollmentService.GetAllByCourseAsync(userId.Value, courseId, filter);

        return TypedResults.Ok(enrollments);
    }

    // get user enrollments
    [Authorize]
    [HttpGet("/enrollments-by-user/{userId:guid}")]
    public async Task<IResult> GetUserEnrollments(Guid userId, EndpointFilter filter, UserId consumerId)
    {
        var enrollments = await _enrollmentService.GetAllByUserAsync(consumerId.Value, userId, filter);

        return TypedResults.Ok(enrollments);
    }
}

/* TODO:
 * 1. Create refund request implementation
 */