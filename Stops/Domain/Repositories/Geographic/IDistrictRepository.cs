using shared.Domain.Repositories;
using stops.Domain.Model.Aggregates.Geographic;

namespace stops.Domain.Repositories.Geographic
{
    public interface IDistrictRepository : IBaseStringRepository<District>
    {
        Task<IEnumerable<District>> FindByFkIdProvinceAsync(int fkIdProvince);
    }
}
