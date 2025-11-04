using stops.Domain.Model.Aggregates.Geographic;
using stops.Domain.Model.Queries.Geographic;
using stops.Domain.Repositories.Geographic;
using stops.Domain.Services.Geographic;

namespace stops.Application.Internal.QueryServices.Geographic
{
    public class ProvinceQueryService(IProvinceRepository provinceRepository) : IProvinceQueryService
    {
        public async Task<IEnumerable<Province>> Handle(GetAllProvincesQuery query)
        {
            return await provinceRepository.ListAsync();
        }
        public async Task<Province?> Handle(GetProvinceByIdQuery query)
        {
            return await provinceRepository.FindByIdIntAsync(query.Id);
        }

        public async Task<IEnumerable<Province>> Handle(GetProvincesByFkIdRegionQuery query)
        {
            return await provinceRepository.FindByFkIdRegionAsync(query.FkIdRegion);
        }
    }
}
