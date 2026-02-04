using Microsoft.AspNetCore.Mvc;
using SimpleKanbanBoards.Business.Exceptions;

namespace SimpleKanbanBoards.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            ProblemDetails problem;

            switch (exception)
            {
                case AppException appEx:
                    problem = new ProblemDetails
                    {
                        Title = appEx.Message,
                        Status = appEx.StatusCode,
                        Type = appEx.GetType().Name
                    };
                    break;

                default:
                    _logger.LogError(exception, "Unhandled exception");

                    problem = new ProblemDetails
                    {
                        Title = "Internal Server Error",
                        Status = StatusCodes.Status500InternalServerError,
                        Detail = "An unexpected error occurred"
                    };
                    break;
            }

            context.Response.StatusCode = problem.Status!.Value;
            context.Response.ContentType = "application/problem+json";

            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}
