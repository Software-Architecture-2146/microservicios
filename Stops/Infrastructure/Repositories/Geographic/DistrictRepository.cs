using shared.Domain.Repositories;
using shared.Infrastructure.Persistences.EFC.Configuration;
using shared.Infrastructure.Persistences.EFC.Repositories;


using stops.Domain.Model.Aggregates.Geographic;
using stops.Domain.Repositories.Geographic;

using Microsoft.EntityFrameworkCore;
using Stops.shared.Infrastructure.Persistences.EFC.Configuration;


namespace stops.Infrastructure.Repositories.Geographic
{
    public class DistrictRepository(AppDbContext context) : BaseStringRepository<District>(context), IDistrictRepository
    {
        public async Task<IEnumerable<District>> FindByFkIdProvinceAsync(int fkIdProvince)
        {
            return await Context.Set<District>()
                .Where(f => f.FkIdProvince == fkIdProvince)
                .ToListAsync();
        }
    }
}
