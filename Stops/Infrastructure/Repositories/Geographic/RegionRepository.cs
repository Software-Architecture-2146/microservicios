using shared.Domain.Repositories;
using shared.Infrastructure.Persistences.EFC.Configuration;
using shared.Infrastructure.Persistences.EFC.Repositories;


using stops.Domain.Model.Aggregates.Geographic;
using stops.Domain.Repositories.Geographic;

using Microsoft.EntityFrameworkCore;
using Stops.shared.Infrastructure.Persistences.EFC.Configuration;


namespace stops.Infrastructure.Repositories.Geographic
{
    public class RegionRepository(AppDbContext context) : BaseStringRepository<Region>(context), IRegionRepository
    {
    }
}
