using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProjectTaskApi.Common.Exceptions
{
    internal sealed class GlobalExceptionHandler(IProblemDetailsService problemDetails,
        ILogger<GlobalExceptionHandler> logger)
        : IExceptionHandler
    {

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError(exception, "Unhandled exception occurred");

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            return await problemDetails.TryWriteAsync(new ProblemDetailsContext
                {
                    HttpContext = httpContext,
                    Exception = exception,
                    ProblemDetails = new ProblemDetails
                    {
                        Status = StatusCodes.Status500InternalServerError,
                        Title = "Server error",
                        Detail = "Internal server error"
                    }
                });
        }
    }
}
