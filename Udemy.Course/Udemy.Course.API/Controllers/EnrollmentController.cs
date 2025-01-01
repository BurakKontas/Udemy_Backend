using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Udemy.Course.API.Controllers;

[Route("/enrollment/v{version:apiVersion}")]
[ApiController]
[ApiVersion("1.0")]
public class EnrollmentController : ControllerBase
{
    // get course enrollment count
    // enroll
    // remove enrollment
    // create refund request
    // get enrollment details
}