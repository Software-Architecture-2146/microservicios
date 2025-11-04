using stops.Domain.Model.Commands.Geographic;
using stops.Interfaces.REST.Resources.Geographic;

namespace stops.Interfaces.REST.Transform.Geographic
{
    public class CreateProvinceCommandFromResourceAssembler
    {
        public static CreateProvinceCommand ToCommandFromResource(CreateProvinceResource resource) =>
            new CreateProvinceCommand(
                resource.Id,
                resource.Name,
                resource.FkIdRegion
            );
    }
}
