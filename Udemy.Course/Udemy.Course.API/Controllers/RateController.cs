using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Udemy.Course.API.Controllers;

[Route("/rate/v{version:apiVersion}")]
[ApiController]
[ApiVersion("1.0")]
public class RateController : ControllerBase
{
    // add rate
    // update rate
    // get rate by course
    // get rates by user
    // get rate details
}