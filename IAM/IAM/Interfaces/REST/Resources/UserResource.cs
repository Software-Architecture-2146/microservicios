using IAM.IAM.Domain.Model.ValueObjects;

namespace IAM.IAM.Interfaces.REST.Resources;

public record UserResource(int Id, string Username, Role Role);