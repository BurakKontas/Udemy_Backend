using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Udemy.Common.ModelBinder;
using Udemy.Course.Domain.Interfaces.Service;

namespace Udemy.Course.API.Controllers;

[Route("/enrollment/v{version:apiVersion}")]
[ApiController]
[ApiVersion("1.0")]
public class EnrollmentController(IEnrollmentService enrollmentService) : ControllerBase
{
    private readonly IEnrollmentService _enrollmentService = enrollmentService;

    // enroll
    [HttpPost("/enroll/{courseId:guid}")]
    public async Task<IResult> Enroll([FromBody] EnrollmentRequest request, Guid courseId)
    {
        var enrollmentId = await _enrollmentService.AddAsync(request.UserId, courseId);

        return TypedResults.Redirect($"get/{enrollmentId}");
    }
    // remove enrollment
    [HttpDelete("/delete/{enrollmentId:guid}")]
    public async Task<IResult> RemoveEnrollment(Guid enrollmentId)
    {
        await _enrollmentService.DeleteAsync(enrollmentId);

        return TypedResults.NoContent();
    }
    // get enrollment details
    [HttpGet("/details/{enrollmentId:guid}")]
    public async Task<IResult> GetEnrollment(Guid enrollmentId)
    {
        var enrollment = await _enrollmentService.GetByIdAsync(enrollmentId);

        return TypedResults.Ok(enrollment);
    }
    // get course enrollments
    [HttpGet("/enrollments/{courseId:guid}")]
    public async Task<IResult> GetEnrollmentCount(Guid courseId, EndpointFilter filter)
    {
        var enrollments = await _enrollmentService.GetAll(courseId, filter);

        return TypedResults.Ok(enrollments);
    }

    // get user enrollments
    [HttpGet("/enrollments-by-user/{userId:guid}")]
    public async Task<IResult> GetUserEnrollments(Guid userId, EndpointFilter filter)
    {
        var enrollments = await _enrollmentService.GetAllByUserAsync(userId, filter);

        return TypedResults.Ok(enrollments);
    }
}

public record EnrollmentRequest(Guid UserId);

/* TODO:
 * 1. Create refund request implementation
 */