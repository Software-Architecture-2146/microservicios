using Company.Api.Infrastructure;
using Company.Api.Domain.Entities;
using Contracts.V1;
using Company.Domain;
using Company.Domain.DTOs;
using Company.Api.Infrastructure;
using CompanyEntity = Company.Api.Domain.Entities.Company;
using MassTransit;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// DB MySQL (puerto 3307 de docker-compose)
var cs = builder.Configuration.GetConnectionString("DefaultConnection")
         ?? "Server=127.0.0.1;Port=3307;Database=frockdb;User Id=root;Password=admin123;SslMode=None;AllowPublicKeyRetrieval=True";
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseMySql(cs, ServerVersion.AutoDetect(cs)));

// RabbitMQ
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("admin");      // <— usa los de tu docker-compose
            h.Password("admin123");
        });
        cfg.ConfigureEndpoints(ctx);
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// crea DB si no existe

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Company API v1");
    c.RoutePrefix = "swagger";
});

// app.UseHttpsRedirection(); // opcional en dev
app.MapGet("/", () => Results.Redirect("/swagger"));

// ENDPOINTS Companies (mínimos)
// GET
app.MapGet("/companies", async (AppDbContext db) =>
    await db.Companies.AsNoTracking().ToListAsync());

// POST
app.MapPost("/companies", async (CreateCompanyDto dto, AppDbContext db, IPublishEndpoint bus) =>
{
    var entity = new CompanyEntity
    {
        Name = dto.Name,
        LogoUrl = dto.LogoUrl,
        CreatedAtUtc = DateTime.UtcNow,
        // Id lo pone MySQL (AUTO_INCREMENT)
        FkIdUser= dto.FkIdUser
    };

    db.Companies.Add(entity);
    await db.SaveChangesAsync(); // aquí ya tienes entity.Id (INT)

    await bus.Publish(new CompanyCreated  // contrato actualizado en paso 4
    {
        CompanyId = entity.Id,            // <- int
        Name = entity.Name,
        CreatedAtUtc = entity.CreatedAtUtc
    });

    return Results.Created($"/api/v1/companies/{entity.Id}", new CompanyDto
    {
        Id = entity.Id,
        Name = entity.Name,
        LogoUrl = entity.LogoUrl,
        FkIdUser = entity.FkIdUser,
        CreatedAtUtc = entity.CreatedAtUtc
    });
});
// PUT  (usar id:int y FindAsync(int))
app.MapPut("/companies/{id:int}", async (int id, CompanyUpdate dto, AppDbContext db, IPublishEndpoint bus) =>
{
    var e = await db.Companies.FindAsync(id);
    if (e is null) return Results.NotFound();

    e.Name = dto.Name;
    e.LogoUrl = dto.LogoUrl;

    await db.SaveChangesAsync();

    // Publica evento versión propiedades (no posicional)
    await bus.Publish(new CompanyUpdated
    {
        CompanyId = e.Id,
        Name = e.Name,
        UpdatedAtUtc = DateTime.UtcNow
    });

    return Results.Ok(e);
});

// DELETE (usar id:int y FindAsync(int))
app.MapDelete("/companies/{id:int}", async (int id, AppDbContext db, IPublishEndpoint bus) =>
{
    var e = await db.Companies.FindAsync(id);
    if (e is null) return Results.NotFound();

    db.Remove(e);
    await db.SaveChangesAsync();

    await bus.Publish(new CompanyDeleted
    {
        CompanyId = e.Id,
        DeletedAtUtc = DateTime.UtcNow
    });

    return Results.NoContent();
});

// justo antes de app.Run();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();

    var existsCompany = await db.Companies.AnyAsync(c => c.Name == "Frock");
    if (!existsCompany)
    {
        db.Companies.Add(new CompanyEntity
        {
            Name = "Frock",
            LogoUrl = "https://example.com/logo.png",
            CreatedAtUtc = DateTime.UtcNow,
            FkIdUser = 1 // <-- ya creaste el user id=1 en MySQL
        });
        await db.SaveChangesAsync();
    }
}

app.Run();

record CompanyCreate(string Name, string? LogoUrl, int OwnerUserId);
record CompanyUpdate(string Name, string? LogoUrl);