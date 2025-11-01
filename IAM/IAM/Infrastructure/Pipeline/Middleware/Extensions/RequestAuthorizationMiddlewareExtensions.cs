using IAM.IAM.Infrastructure.Pipeline.Middleware.Components;

namespace IAM.IAM.Infrastructure.Pipeline.Middleware.Extensions;

public static class RequestAuthorizationMiddlewareExtensions
{
    /**
     * UseRequestAuthorization extension method is used to register RequestAuthorizationMiddleware in the ASP.NET Core pipeline.
     */
    public static IApplicationBuilder UseRequestAuthorization(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestAuthorizationMiddleware>();
    }
}