using stops.Domain.Model.Aggregates.Geographic;
using stops.Domain.Model.Queries.Geographic;
using stops.Domain.Repositories.Geographic;
using stops.Domain.Services.Geographic;

namespace stops.Application.Internal.QueryServices.Geographic
{
    public class DistrictQueryService(IDistrictRepository districtRepository) : IDistrictQueryService
    {
        public async Task<IEnumerable<District>> Handle(GetAllDistrictsQuery query)
        {
            return await districtRepository.ListAsync();
        }
        public async Task<District?> Handle(GetDistrictByIdQuery query)
        {
            return await districtRepository.FindByIdIntAsync(query.Id);
        }        
        public async Task<IEnumerable<District>> Handle(GetDistrictsByFkIdProvinceQuery query)
        {
            return await districtRepository.FindByFkIdProvinceAsync(query.FkIdProvince);
        }
    }
}
