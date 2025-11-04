using stops.Domain.Model.Commands.Geographic;
using stops.Interfaces.REST.Resources.Geographic;

namespace stops.Interfaces.REST.Transform.Geographic
{
    public class CreateDistrictCommandFromResourceAssembler
    {
        public static CreateDistrictCommand ToCommandFromResource(CreateDistrictResource resource) =>
            new CreateDistrictCommand(
                resource.Id,
                resource.Name,
                resource.FkIdProvince
            );
    }
}
