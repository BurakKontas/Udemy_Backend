using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Udemy.Course.API.Controllers;

[Route("/lesson/v{version:apiVersion}")]
[ApiController]
[ApiVersion("1.0")]
public class LessonController : ControllerBase
{
    // add lesson
    // update lesson
    // delete lesson
    // hide lesson
    // get lesson details
    // get lessons by course
}