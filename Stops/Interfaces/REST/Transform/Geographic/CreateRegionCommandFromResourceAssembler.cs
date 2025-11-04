using stops.Domain.Model.Commands.Geographic;
using stops.Interfaces.REST.Resources.Geographic;

namespace stops.Interfaces.REST.Transform.Geographic
{
    public class CreateRegionCommandFromResourceAssembler
    {
        public static CreateRegionCommand ToCommandFromResource(CreateRegionResource resource) =>
            new CreateRegionCommand(
                resource.Id,
                resource.Name
                );
    }
}
