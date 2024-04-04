using Microsoft.AspNetCore.Diagnostics;
using System.Diagnostics;

namespace OrderService.API
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

            _logger.LogError(
                   exception,
                   "Could not process a request on machine {Machine}. Traceid: {TraceId}",
                   Environment.MachineName,
                   traceId);

            var (statusCode, title) = MapException(exception);

            await Results.Problem(
                   title: title, // can update error message.
                   statusCode: statusCode,
                   extensions: new Dictionary<string, object?>
                   {
                        {"traceId", Activity.Current?.Id }
                   }
                   ).ExecuteAsync(httpContext);

            return true;
        }

        private static (int StatusCode, string Title) MapException(Exception exception)
        {
            return exception switch
            {
                ArgumentOutOfRangeException => (StatusCodes.Status400BadRequest, exception.Message),
                _ => (StatusCodes.Status500InternalServerError, "We made a mistake while processing you request, we are working on it.")

                // Can add more exception as per need.
            };
        }
    }
}
