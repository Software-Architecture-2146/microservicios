
using IAM.IAM.Domain.Model.ValueObjects;

namespace IAM.IAM.Interfaces.REST.Resources;

public record AuthenticatedUserResource(int Id, string Username, Role Role, string Token);