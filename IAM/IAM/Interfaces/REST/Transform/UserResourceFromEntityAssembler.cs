using IAM.IAM.Domain.Model.Aggregates;
using IAM.IAM.Interfaces.REST.Resources;

namespace IAM.IAM.Interfaces.REST.Transform;

public static class UserResourceFromEntityAssembler
{
    public static UserResource ToResourceFromEntity(User user)
    {
        return new UserResource(user.Id, user.Username, user.Role);
    }
}