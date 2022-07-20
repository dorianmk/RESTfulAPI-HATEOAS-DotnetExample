using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace WebAPI.ActionFilters
{
    public class ResponseExceptionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                var result = new ObjectResult("Exception has been thrown")
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };

                if (context.Exception is ValidationException validationException)
                {
                    if (validationException.InnerException is AggregateException aggregateException)
                        result.Value = string.Join(Environment.NewLine, aggregateException.InnerExceptions.Select(x => x.Message));
                }

                context.Result = result;
                context.ExceptionHandled = true;
            }
        }
    }
}
