using shared.Domain.Repositories;
using stops.Domain.Model.Aggregates.Geographic;
using stops.Domain.Model.Commands.Geographic;
using stops.Domain.Repositories.Geographic;
using stops.Domain.Services.Geographic;

namespace stops.Application.Internal.CommandServices.Geographic
{
    public class RegionCommandService(IRegionRepository regionRepository, IUnitOfWork unitOfWork) : IRegionCommandService
    {
        public async Task<Region?> Handle(CreateRegionCommand command)
        {
            var existingRegion = 
                await regionRepository.FindByIdIntAsync(command.Id);
            if (existingRegion != null)
            {
                throw new Exception($"Region already exists with that Id.");
            }
            var newRegion = new Region(command);
            try
            {
                await regionRepository.AddAsync(newRegion);
                await unitOfWork.CompleteAsync();
                return newRegion;
            }
            catch (Exception e)
            {
                // logger?.LogError(e, "Error creating region with name {RegionName} for locality {LocalityId}.", command.Name, command.FkIdLocality);
                return null; // Signal failure to the controller
            }
        }

    }
}
