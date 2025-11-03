using System.Net.Mime;
using Frock_backend.IAM.Application.Internal.OutboundServices;
using Frock_backend.IAM.Domain.Model.Queries;
using Frock_backend.IAM.Domain.Services;
using Frock_backend.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using Frock_backend.IAM.Interfaces.REST.Resources;
using Frock_backend.IAM.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Frock_backend.IAM.Interfaces.REST;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Authentication endpoints")]
public class AuthenticationController(
    IUserCommandService userCommandService,
    ITokenService tokenService,
    IUserQueryService userQueryService) : ControllerBase
{
    /**
     * <summary>
     *     Sign in endpoint. It allows authenticating a user
     * </summary>
     * <param name="signInResource">The sign-in resource containing username and password.</param>
     * <returns>The authenticated user resource, including a JWT token</returns>
     */
    [HttpPost("sign-in")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Sign in",
        Description = "Sign in a user",
        OperationId = "SignIn")]
    [SwaggerResponse(StatusCodes.Status200OK, "The user was authenticated", typeof(AuthenticatedUserResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request data")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid credentials")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error")]
    public async Task<IActionResult> SignIn([FromBody] SignInResource signInResource)
    {
        try
        {
            if (signInResource == null)
                return BadRequest("Sign in data is required");

            var signInCommand = SignInCommandFromResourceAssembler.ToCommandFromResource(signInResource);
            var authenticatedUser = await userCommandService.Handle(signInCommand);
            var resource =
                AuthenticatedUserResourceFromEntityAssembler.ToResourceFromEntity(authenticatedUser.user,
                    authenticatedUser.token);
            return Ok(resource);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Sign in error: {ex}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred during sign in");
        }
    }

    /**
     * <summary>
     *     Sign up endpoint. It allows creating a new user
     * </summary>
     * <param name="signUpResource">The sign-up resource containing username and password.</param>
     * <returns>A confirmation message on successful creation.</returns>
     */
    [HttpPost("sign-up")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Sign-up",
        Description = "Sign up a new user",
        OperationId = "SignUp")]
    [SwaggerResponse(StatusCodes.Status200OK, "The user was created successfully")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request data or user already exists")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error")]
    public async Task<IActionResult> SignUp([FromBody] SignUpResource signUpResource)
    {
        try
        {
            if (signUpResource == null)
                return BadRequest("Sign up data is required");

            var signUpCommand = SignUpCommandFromResourceAssembler.ToCommandFromResource(signUpResource);
            await userCommandService.Handle(signUpCommand);
            return Ok(new { message = "User created successfully" });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex) when (ex.Message.Contains("already"))
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Sign up error: {ex}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred during sign up");
        }
    }

    /**
     * <summary>
     *     Validate token endpoint
     * </summary>
     * <param name="token">The JWT token to validate</param>
     * <returns>Token validation result</returns>
     */
    [HttpPost("validate-token")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Validate JWT token",
        Description = "Validate a JWT token and return user information if valid",
        OperationId = "ValidateToken")]
    [SwaggerResponse(StatusCodes.Status200OK, "Token is valid")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid token")]
    public async Task<IActionResult> ValidateToken([FromBody] TokenValidationResource tokenResource)
    {
        try
        {
            if (tokenResource?.Token == null)
                return BadRequest("Token is required");

            var userId = await tokenService.ValidateToken(tokenResource.Token);
            if (userId == null)
                return Unauthorized("Invalid or expired token");

            var getUserByIdQuery = new GetUserByIdQuery(userId.Value);
            var user = await userQueryService.Handle(getUserByIdQuery);
            
            if (user == null)
                return Unauthorized("User not found");

            var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(user);
            return Ok(new { 
                message = "Token is valid", 
                user = userResource,
                userId = userId.Value
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Token validation error: {ex}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred during token validation");
        }
    }
}