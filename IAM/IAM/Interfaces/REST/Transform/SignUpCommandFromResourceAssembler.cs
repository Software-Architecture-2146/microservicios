using IAM.IAM.Domain.Model.Commands;
using IAM.IAM.Interfaces.REST.Resources;

namespace IAM.IAM.Interfaces.REST.Transform;

public static class SignUpCommandFromResourceAssembler
{
    public static SignUpCommand ToCommandFromResource(SignUpResource resource)
    {
        return new SignUpCommand
        {
            Email = resource.Email,
            Username = resource.Username,
            Password = resource.Password,
            Role = resource.Role 
        };
    }
}
