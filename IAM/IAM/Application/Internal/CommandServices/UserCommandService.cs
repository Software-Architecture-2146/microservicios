using Frock_backend.IAM.Application.Internal.OutboundServices;
using Frock_backend.IAM.Domain.Model.Aggregates;
using Frock_backend.IAM.Domain.Model.Commands;
using Frock_backend.IAM.Domain.Repositories;
using Frock_backend.IAM.Domain.Services;
using Frock_backend.shared.Domain.Repositories;

namespace Frock_backend.IAM.Application.Internal.CommandServices;
public class UserCommandService(
    IUserRepository userRepository,
    ITokenService tokenService,
    IHashingService hashingService,
    IUnitOfWork unitOfWork)
    : IUserCommandService
{
    /**
     * <summary>
     *     Handle sign in command
     * </summary>
     * <param name="command">The sign in command</param>
     * <returns>The authenticated user and the JWT token</returns>
     */
    public async Task<(User user, string token)> Handle(SignInCommand command)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        if (string.IsNullOrWhiteSpace(command.Email))
            throw new ArgumentException("Email is required", nameof(command.Email));

        if (string.IsNullOrWhiteSpace(command.Password))
            throw new ArgumentException("Password is required", nameof(command.Password));

        var user = await userRepository.FindByEmailAsync(command.Email);

        if (user == null)
            throw new UnauthorizedAccessException("Invalid email or password");

        if (string.IsNullOrEmpty(user.PasswordHash) || 
            !hashingService.VerifyPassword(command.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password");

        var token = tokenService.GenerateToken(user);

        return (user, token);
    }

    /**
     * <summary>
     *     Handle sign up command
     * </summary>
     * <param name="command">The sign up command</param>
     * <returns>A confirmation message on successful creation.</returns>
     */
    public async Task Handle(SignUpCommand command)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        if (string.IsNullOrWhiteSpace(command.Email))
            throw new ArgumentException("Email is required", nameof(command.Email));

        if (string.IsNullOrWhiteSpace(command.Username))
            throw new ArgumentException("Username is required", nameof(command.Username));

        if (string.IsNullOrWhiteSpace(command.Password))
            throw new ArgumentException("Password is required", nameof(command.Password));

        if (command.Password.Length < 6)
            throw new ArgumentException("Password must be at least 6 characters long", nameof(command.Password));

        if (await userRepository.ExistsByEmail(command.Email))
            throw new Exception($"Email '{command.Email}' is already registered");

        var hashedPassword = hashingService.HashPassword(command.Password);
        var user = new User(command.Email, command.Username, hashedPassword, command.Role);

        try
        {
            await userRepository.AddAsync(user);
            await unitOfWork.CompleteAsync();
        }
        catch (Exception e)
        {
            throw new Exception($"An error occurred while creating user: {e.Message}");
        }
    }
}
