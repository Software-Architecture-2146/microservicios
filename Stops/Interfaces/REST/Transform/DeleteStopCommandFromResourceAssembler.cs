using stops.Domain.Model.Commands;
using stops.Interfaces.REST.Resources;

namespace stops.Interfaces.REST.Transform
{
    public class DeleteStopCommandFromResourceAssembler
    {
        public static DeleteStopCommand ToCommandFromResource(DeleteStopResource resource)
        {
            return new DeleteStopCommand(resource.Id);
        }

    }
}
