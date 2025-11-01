using System.Text.Json.Serialization;
using IAM.IAM.Domain.Model.ValueObjects;

namespace IAM.IAM.Domain.Model.Aggregates;

public class User (string email, string username, string passwordHash, Role role)
{
    public User() : this(string.Empty, string.Empty, string.Empty, Role.Traveller)
    {
    }

    public int Id { get; }
    public string Email { get; private set; } = email;
    public string Username { get; private set; } = username;
    public Role Role { get; private set; } = role;

    [JsonIgnore] public string PasswordHash { get; private set; } = passwordHash;
}