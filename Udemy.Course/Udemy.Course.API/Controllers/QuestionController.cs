using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Udemy.Course.API.Controllers;

[Route("/v{version:apiVersion}/question")]
[ApiController]
[ApiVersion("1.0")]
public class QuestionController : ControllerBase
{
    // get questions by course
    // get question details
    // make question
    // edit question
    // delete question
    // hide question
    // answer question
    // get answers by question
    // get answer details
}