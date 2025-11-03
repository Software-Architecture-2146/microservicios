using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

//SHARED
using IAM.Shared.Infrastructure.Persistences.EFC.Configuration; 
using IAM.Shared.Infrastructure.Persistences.EFC.Repositories;
using IAM.Shared.Infrastructure.Interfaces.ASP.Configuration;
using IAM.Shared.Domain.Repositories;

//IAM - Adrian
using IAM.IAM.Application.Internal.CommandServices;
using IAM.IAM.Application.Internal.OutboundServices;
using IAM.IAM.Application.Internal.QueryServices;

using IAM.IAM.Domain.Repositories;
using IAM.IAM.Domain.Services;
using IAM.IAM.Infrastructure.Persistence.EFC.Repositories;

using IAM.IAM.Infrastructure.Hashing.Hashing.BCrypt.Services;
using IAM.IAM.Infrastructure.Pipeline.Middleware.Extensions;
using IAM.IAM.Infrastructure.Tokens.JWT.Configuration;
using IAM.IAM.Infrastructure.Tokens.JWT.Services;

using IAM.IAM.Interfaces.ACL;
using IAM.IAM.Interfaces.ACL.Services;

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
            Title = "Frock_Backend",
            Version = "v1",
            Description = "Frock Backend API",
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

// IAM Bounded Context Injection Configuration
// TokenSettings Configuration
builder.Services.Configure<TokenSetting>(builder.Configuration.GetSection("TokenSettings"));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IHashingService, HashingService>();
builder.Services.AddScoped<IIamContextFacade, IamContextFacade>();

var app = builder.Build();
app.UseCors();

// Verify Database Objects are created
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
app.UseSwagger(c =>
{
    c.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi2_0;
});

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
    c.RoutePrefix = string.Empty; // Opcional: para que Swagger sea la p�gina ra�z
    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
});

app.UseHttpsRedirection();
app.UseRouting(); // Si no está implícito
app.UseRequestAuthorization(); // Tu middleware personalizado
app.UseAuthorization(); // Authorization de ASP.NET Core
app.MapControllers();

app.Run();