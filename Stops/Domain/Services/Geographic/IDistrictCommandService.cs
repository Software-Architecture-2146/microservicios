using stops.Domain.Model.Commands.Geographic;
using stops.Domain.Model.Aggregates.Geographic;

namespace stops.Domain.Services.Geographic
{
    public interface IDistrictCommandService
    {
        Task<District?> Handle(CreateDistrictCommand command);
    }
}
