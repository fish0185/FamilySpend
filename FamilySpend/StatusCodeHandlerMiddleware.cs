using Microsoft.AspNetCore.Mvc;

namespace FamilySpend;

using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class StatusCodeHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IProblemDetailsService _problemDetailsService;

    public StatusCodeHandlerMiddleware(
        RequestDelegate next,
        IProblemDetailsService problemDetailsService)
    {
        _next = next;
        _problemDetailsService = problemDetailsService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // 1. Let the downstream middleware execute first
        await _next(context);

        // 2. Check the status code on the return path
        if (context.Response.StatusCode is StatusCodes.Status401Unauthorized or StatusCodes.Status403Forbidden 
            && !context.Response.HasStarted)
        {
            // Clear content if necessary and rewrite response
            context.Response.ContentType = "application/problem+json";
            var problemDetails = new ProblemDetails
            {
                Status = context.Response.StatusCode,
                Title = "An unexpected error occurred",
                Detail = context.Response.StatusCode.ToString()
            };
            await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = context,
                ProblemDetails = problemDetails
            });
        }
    }
}