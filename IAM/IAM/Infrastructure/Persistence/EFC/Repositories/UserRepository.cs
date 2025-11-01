using IAM.IAM.Domain.Model.Aggregates;
using IAM.IAM.Domain.Repositories;
using IAM.Shared.Infrastructure.Persistences.EFC.Configuration;
using IAM.Shared.Infrastructure.Persistences.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IAM.IAM.Infrastructure.Persistence.EFC.Repositories;

public class UserRepository(AppDbContext context) : BaseRepository<User>(context), IUserRepository
{
    public async Task<User?> FindByUsernameAsync(string username)
    {
        return await Context.Set<User>().FirstOrDefaultAsync(user => user.Username.Equals(username));
    }

    public bool ExistsByUsername(string username)
    {
        return Context.Set<User>().Any(user => user.Username.Equals(username));
    }

    public async Task<User?> FindByEmailAsync(string email)
    {
        return await Context.Set<User>().FirstOrDefaultAsync(user => user.Email.Equals(email));
    }

    public async Task<bool> ExistsByEmail(string email)
    {
        return await Context.Set<User>().AnyAsync(user => user.Email.Equals(email));
    }
}