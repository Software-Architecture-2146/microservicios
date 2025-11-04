using shared.Domain.Repositories;
using stops.Domain.Model.Aggregates.Geographic;

namespace stops.Domain.Repositories.Geographic
{
    public interface IProvinceRepository : IBaseStringRepository<Province>
    {
        Task<IEnumerable<Province>> FindByFkIdRegionAsync(int fkIdRegion);
    }
}
