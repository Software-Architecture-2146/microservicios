
using IAM.IAM.Domain.Model.ValueObjects;

namespace IAM.IAM.Interfaces.ACL;

public interface IIamContextFacade
{
    Task<int> CreateUser(string username, string email, string password, Role role);
    Task<int> FetchUserIdByUsername(string username);

    Task<int> FetchUserIdByEmail(string email);

    Task<string> FetchUsernameByUserId(int userId);

    Task<string> FetchEmailByUserId(int userId);
}
