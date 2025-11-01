using IAM.Shared.Domain.Repositories;
using IAM.Shared.Infrastructure.Persistences.EFC.Configuration;

namespace IAM.Shared.Infrastructure.Persistences.EFC.Repositories;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public async Task CompleteAsync()
    {
        await context.SaveChangesAsync();
    }
}