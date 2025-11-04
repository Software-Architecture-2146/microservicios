using stops.Domain.Model.Aggregates.Geographic;
using stops.Domain.Model.Queries.Geographic;
using stops.Domain.Repositories.Geographic;
using stops.Domain.Services.Geographic;

namespace stops.Application.Internal.QueryServices.Geographic
{
    public class RegionQueryService(IRegionRepository regionRepository) : IRegionQueryService
    {
        public async Task<IEnumerable<Region>> Handle(GetAllRegionsQuery query)
        {
            return await regionRepository.ListAsync();
        }
        public async Task<Region?> Handle(GetRegionByIdQuery query)
        {
            return await regionRepository.FindByIdIntAsync(query.Id);
        }
    }
}
