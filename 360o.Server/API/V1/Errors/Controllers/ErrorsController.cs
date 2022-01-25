using _360o.Server.API.V1.Errors.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace _360o.Server.API.V1.Errors
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ErrorsController : ControllerBase
    {
        [Route("/error")]
        public ActionResult Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context.Error;

            if (exception is ValidationException validationException)
            {
                return Problem(detail: validationException.Message, statusCode: (int)HttpStatusCode.BadRequest, title: ErrorCode.InvalidRequest.ToString());
            }
            else
            {
                return Problem();
                ;
            }

        }
    }
}
