using EntityFrameworkCore.CreatedUpdatedDate.Extensions;
using IAM.IAM.Domain.Model.Aggregates;
using IAM.Shared.Infrastructure.Persistences.EFC.Configuration.Extensions;
using Microsoft.EntityFrameworkCore;

namespace IAM.Shared.Infrastructure.Persistences.EFC.Configuration;

public class AppDbContext (DbContextOptions options) : DbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.AddCreatedUpdatedInterceptor();
            base.OnConfiguring(builder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // IAM Context
            builder.Entity<User>().HasKey(u => u.Id);
            builder.Entity<User>().Property(u => u.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<User>().Property(u => u.Username).IsRequired();
            builder.Entity<User>().Property(u => u.PasswordHash).IsRequired();
            builder.Entity<User>().Property(u => u.Role).HasConversion<string>().IsRequired();
            
            builder.UseSnakeCaseNamingConvention();
        }
}