using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Udemy.Course.API.Controllers;

[Route("/comment/v{version:apiVersion}")]
[ApiController]
[ApiVersion("1.0")]
public class CommentController : ControllerBase
{
    // get comments by course
    // make comment
    // edit comments
}