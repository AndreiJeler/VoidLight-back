using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;
using VoidLight.Infrastructure.Common.Exceptions;
using VoidLight.Infrastructure.Common;


namespace VoidLight.Web.Infrastructure.Middlewares
{
    public class ExceptionMiddleware
    {
        public async Task Invoke(HttpContext context)
        {
            var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
            if (contextFeature != null && contextFeature.Error != null)
            {
                context.Response.ContentType = "application/json";
                var problemDetails = GetProblemDetails(context, contextFeature);
                await context.Response.WriteAsync(JsonConvert.SerializeObject(problemDetails));
            }
        }

        private static ProblemDetails GetProblemDetails(HttpContext context, IExceptionHandlerFeature exception)
        {
            if (!Extensions.IsSystemException(exception.Error))
            {
                if (exception.Error is ApiExceptionBase exceptionBase)
                {
                    return new ExceptionProblemDetails(exceptionBase);
                }
            }

            return new ProblemDetails
            {
                Status = context.Response.StatusCode,
                Title = "Exception",
                Detail = exception.Error.Message
            };
        }
    }
}
