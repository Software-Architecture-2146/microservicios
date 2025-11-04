using Company.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using CompanyEntity = Company.Api.Domain.Entities.Company;
namespace Company.Api.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) { }
    public DbSet<CompanyEntity> Companies => Set<CompanyEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Domain.Entities.Company>(e =>
        {
            e.ToTable("companies");

            e.HasKey(x => x.Id);

            e.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd(); // <- deja que MySQL lo asigne

            e.Property(x => x.Name)
                .HasColumnName("name").HasMaxLength(200).IsRequired();

            e.Property(x => x.LogoUrl)
                .HasColumnName("logo_url").HasMaxLength(500);

            e.Property(x => x.FkIdUser)
                .HasColumnName("fk_id_user");

            e.Property(x => x.CreatedAtUtc)
                .HasColumnName("created_at_utc")
                .HasColumnType("datetime(6)")
                .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
            
        });
    }

}