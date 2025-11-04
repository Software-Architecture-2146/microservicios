using shared.Domain.Repositories;
using stops.Domain.Model.Aggregates;

namespace stops.Domain.Repositories
{
    public interface IStopRepository : IBaseRepository<Stop>
    {
        Task<IEnumerable<Stop>> FindByFkIdCompanyAsync(int fkIdCompany);
        Task<IEnumerable<Stop>> FindByFkIdDistrictAsync(int fkIdDistrict);
        Task<Stop?> FindByNameAndFkIdDistrictAsync(string name, int fkIdDistrict);

        Task<Stop?> FindByNameAndFkIdCompanyAsync(string name, int fkIdCompany);
    }
}
