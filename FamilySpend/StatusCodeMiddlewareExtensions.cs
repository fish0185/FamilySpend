namespace FamilySpend;

using Microsoft.AspNetCore.Builder;

public static class StatusCodeMiddlewareExtensions
{
    public static IApplicationBuilder UseStatusCodeHandler(this IApplicationBuilder app)
    {
        return app.UseMiddleware<StatusCodeHandlerMiddleware>();
    }
}