using IAM.IAM.Domain.Model.Aggregates;
using IAM.IAM.Interfaces.REST.Resources;

namespace IAM.IAM.Interfaces.REST.Transform;

public static class AuthenticatedUserResourceFromEntityAssembler
{
    public static AuthenticatedUserResource ToResourceFromEntity(
        User user, string token)
    {
        return new AuthenticatedUserResource(user.Id, user.Username, user.Role, token);
    }
}