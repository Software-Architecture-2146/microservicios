using Frock_backend.IAM.Application.Internal.OutboundServices;
using Frock_backend.IAM.Domain.Model.Queries;
using Frock_backend.IAM.Domain.Services;
using Frock_backend.IAM.Infrastructure.Pipeline.Middleware.Attributes;

namespace Frock_backend.IAM.Infrastructure.Pipeline.Middleware.Components;

public class RequestAuthorizationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext context,
        IUserQueryService userQueryService,
        ITokenService tokenService)
    {
        // Skip authorization for OPTIONS requests (CORS preflight)
        if (context.Request.Method == HttpMethods.Options)
        {
            await next(context);
            return;
        }

        // Check if endpoint allows anonymous access
        var endpoint = context.GetEndpoint();
        if (endpoint != null)
        {
            var allowAnonymous = endpoint.Metadata
                .Any(m => m.GetType() == typeof(AllowAnonymousAttribute));
            
            if (allowAnonymous)
            {
                await next(context);
                return;
            }
        }

        // Get token from Authorization header
        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Missing or invalid authorization header");
            return;
        }

        var token = authHeader["Bearer ".Length..].Trim();

        // Validate token
        var userId = await tokenService.ValidateToken(token);
        if (userId == null)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Invalid or expired token");
            return;
        }

        // Get user by id and set in context
        try
        {
            var getUserByIdQuery = new GetUserByIdQuery(userId.Value);
            var user = await userQueryService.Handle(getUserByIdQuery);
            
            if (user == null)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("User not found");
                return;
            }

            context.Items["User"] = user;
            context.Items["UserId"] = userId.Value;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Authorization error: {ex}");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Authorization error");
            return;
        }

        await next(context);
    }
}