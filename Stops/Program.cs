using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

//SHARED
using Stops.shared.Infrastructure.Persistences.EFC.Configuration;
using Stops.shared.Infrastructure.Persistences.EFC.Repositories;
using shared.Infrastructure.Interfaces.ASP.Configuration;
using shared.Domain.Repositories;
using shared.Domain.Services;
using shared.Infrastructure.Configuration;
using shared.Infrastructure.Services;
using stops.Application.External;

//STOPS - Amir

using stops.Application.Internal.CommandServices;
using stops.Application.Internal.QueryServices;

using stops.Domain.Repositories;
using stops.Domain.Services;

using stops.Infrastructure.Repositories;

//GEOGRAPHIC
using stops.Application.Internal.CommandServices.Geographic;
using stops.Application.Internal.QueryServices.Geographic;

using stops.Domain.Repositories.Geographic;
using stops.Domain.Services.Geographic;

using stops.Infrastructure.Repositories.Geographic;

using stops.Infrastructure.Seeding;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Configure Lower Case URLs
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Configure Kebab Case Route Naming Convention
builder.Services.AddControllers(options => options.Conventions.Add(new KebabCaseRouteNamingConvention()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
     options.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "Microservice Stops",
            Version = "v1",
            Description = "Frock Backend API - Stops",
            TermsOfService = new Uri("https://acme-learning.com/tos"),
            Contact = new OpenApiContact
            {
                Name = "frock Studios",
                Email = "frockWEB.com"
            },
            License = new OpenApiLicense
            {
                Name = "Apache 2.0",
                Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0.html")
            }
        });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            Array.Empty<string>()
        }
    });
});

/// <summary>
/// Obtiene la cadena de conexión a la base de datos MySQL desde la configuración de la aplicación.
/// </summary>
/// <remarks>
/// El valor se extrae de la sección "ConnectionStrings" del archivo `appsettings.json`,
/// buscando la clave "DefaultConnection".
/// </remarks>
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (connectionString is null)
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
}

// Configure Database Context and Logging Levels
if (builder.Environment.IsDevelopment())
    builder.Services.AddDbContext<AppDbContext>(
        options =>
        {
            options.UseMySQL(connectionString)
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        });
else if (builder.Environment.IsProduction())
    builder.Services.AddDbContext<AppDbContext>(
        options =>
        {
            options.UseMySQL(connectionString)
                .LogTo(Console.WriteLine, LogLevel.Error)
                .EnableDetailedErrors();
        });


// Configure Dependency Injection

// Shared Bounded Context Injection Configuration
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


//Geographic
builder.Services.AddScoped<IRegionRepository, RegionRepository>();
    builder.Services.AddScoped<IRegionCommandService, RegionCommandService>();
    builder.Services.AddScoped<IRegionQueryService, RegionQueryService>();
        /**/
    builder.Services.AddScoped<IProvinceRepository, ProvinceRepository>();
    builder.Services.AddScoped<IProvinceCommandService, ProvinceCommandService>();
    builder.Services.AddScoped<IProvinceQueryService, ProvinceQueryService>();
        /**/
    builder.Services.AddScoped<IDistrictRepository, DistrictRepository>();
    builder.Services.AddScoped<IDistrictCommandService, DistrictCommandService>();
    builder.Services.AddScoped<IDistrictQueryService, DistrictQueryService>();
        /**/

//Stops
    builder.Services.AddScoped<IStopRepository, StopRepository>();
    builder.Services.AddScoped<IStopCommandService, StopCommandService>();
    builder.Services.AddScoped<IStopQueryService, StopQueryService>();


//GEOSERVICE
    builder.Services.AddHttpClient<IGeoImportService, GeoImportService>(client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["GeoApi:BaseUrl"]);
    });
//Seeding Service Geographic Data
// Datos iniciales fijos de datos geográficos
builder.Services.AddScoped<GeographicDataSeeder>();


//CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173", "https://deft-tapioca-c27a9c.netlify.app", "https://frock-front-end.vercel.app")//ajustar
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Cloudinary Configuration
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();

var app = builder.Build();

app.UseCors();

// Verify Database Objects are created
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();

  // Seed initial geographic data
    try
    {
        var seeder = services.GetRequiredService<GeographicDataSeeder>();
        await seeder.SeedDataAsync();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error durante la carga de datos iniciales.");
    }
}

// Configure the HTTP request pipeline.
app.UseSwagger(c =>
{
  
});

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
    c.RoutePrefix = string.Empty; // Opcional: para que Swagger sea la p�gina ra�z
    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
});

app.UseHttpsRedirection();
app.UseRouting(); // Si no está implícito
app.UseAuthorization(); // Authorization de ASP.NET Core
app.MapControllers();

app.Run();
