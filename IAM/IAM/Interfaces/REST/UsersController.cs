using System.Net.Mime;
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
[SwaggerTag("Available User Management endpoints")]
public class UsersController(IUserQueryService userQueryService) : BaseController
{
    /**
     * <summary>
     *     Get all users endpoint
     * </summary>
     * <returns>List of all users</returns>
     */
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all users",
        Description = "Get a list of all users",
        OperationId = "GetAllUsers")]
    [SwaggerResponse(StatusCodes.Status200OK, "Users retrieved successfully", typeof(IEnumerable<UserResource>))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized access")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error")]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var getAllUsersQuery = new GetAllUsersQuery();
            var users = await userQueryService.Handle(getAllUsersQuery);
            var userResources = users.Select(UserResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(userResources);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Get all users error: {ex}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving users");
        }
    }

    /**
     * <summary>
     *     Get user by ID endpoint
     * </summary>
     * <param name="id">User ID</param>
     * <returns>User details</returns>
     */
    [HttpGet("{id:int}")]
    [SwaggerOperation(
        Summary = "Get user by ID",
        Description = "Get a user by their ID",
        OperationId = "GetUserById")]
    [SwaggerResponse(StatusCodes.Status200OK, "User retrieved successfully", typeof(UserResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "User not found")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized access")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error")]
    public async Task<IActionResult> GetUserById([FromRoute] int id)
    {
        try
        {
            var getUserByIdQuery = new GetUserByIdQuery(id);
            var user = await userQueryService.Handle(getUserByIdQuery);

            if (user == null)
                return NotFound($"User with ID {id} not found");

            var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(user);
            return Ok(userResource);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Get user by ID error: {ex}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the user");
        }
    }

    /**
     * <summary>
     *     Get user by email endpoint
     * </summary>
     * <param name="email">User email</param>
     * <returns>User details</returns>
     */
    [HttpGet("email/{email}")]
    [SwaggerOperation(
        Summary = "Get user by email",
        Description = "Get a user by their email address",
        OperationId = "GetUserByEmail")]
    [SwaggerResponse(StatusCodes.Status200OK, "User retrieved successfully", typeof(UserResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "User not found")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized access")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error")]
    public async Task<IActionResult> GetUserByEmail([FromRoute] string email)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest("Email is required");

            var getUserByEmailQuery = new GetUserByEmailQuery(email);
            var user = await userQueryService.Handle(getUserByEmailQuery);

            if (user == null)
                return NotFound($"User with email {email} not found");

            var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(user);
            return Ok(userResource);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Get user by email error: {ex}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the user");
        }
    }

    /**
     * <summary>
     *     Get current user profile endpoint
     * </summary>
     * <returns>Current authenticated user details</returns>
     */
    [HttpGet("profile")]
    [SwaggerOperation(
        Summary = "Get current user profile",
        Description = "Get the profile of the currently authenticated user",
        OperationId = "GetCurrentUserProfile")]
    [SwaggerResponse(StatusCodes.Status200OK, "User profile retrieved successfully", typeof(UserResource))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized access")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error")]
    public async Task<IActionResult> GetCurrentUserProfile()
    {
        try
        {
            var user = GetAuthenticatedUser();
            if (user == null)
                return Unauthorized("User not authenticated");

            var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(user);
            return Ok(userResource);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Get current user profile error: {ex}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the user profile");
        }
    }
}