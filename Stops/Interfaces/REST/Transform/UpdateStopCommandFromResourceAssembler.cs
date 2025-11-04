using stops.Domain.Model.Commands;
using stops.Interfaces.REST.Resources;

namespace stops.Interfaces.REST.Transform
{
    public class UpdateStopCommandFromResourceAssembler
    {
        public static UpdateStopCommand ToCommandFromResource(UpdateStopResource resource)
        {
            return new UpdateStopCommand(
                resource.Id,
                resource.Name,
                resource.GoogleMapsUrl,
                resource.ImageUrl,
                resource.Phone,
                resource.FkIdCompany,
                resource.Address,
                resource.Reference,
                resource.FkIdDistrict
            );
        }
    }
}
