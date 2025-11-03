using Frock_backend.IAM.Domain.Model.Aggregates;
using Microsoft.AspNetCore.Mvc;

namespace Frock_backend.IAM.Interfaces.REST;

/// <summary>
/// Base controller that provides access to the authenticated user
/// </summary>
public abstract class BaseController : ControllerBase
{
    /// <summary>
    /// Get the authenticated user from the HTTP context
    /// </summary>
    /// <returns>The authenticated user or null if not authenticated</returns>
    protected User? GetAuthenticatedUser()
    {
        return HttpContext.Items["User"] as User;
    }

    /// <summary>
    /// Get the authenticated user ID from the HTTP context
    /// </summary>
    /// <returns>The authenticated user ID or null if not authenticated</returns>
    protected int? GetAuthenticatedUserId()
    {
        return HttpContext.Items["UserId"] as int?;
    }

    /// <summary>
    /// Check if the current user is authenticated
    /// </summary>
    /// <returns>True if authenticated, false otherwise</returns>
    protected bool IsAuthenticated()
    {
        return GetAuthenticatedUser() != null;
    }
}
