using Audit.Api.Consumers;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// MassTransit -> RabbitMQ
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CompanyCreatedConsumer>();

    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("admin");   // <- usa los mismos que funcionan en Company
            h.Password("admin123");
        });
        cfg.ReceiveEndpoint("audit-company-events", e =>
        {
            e.PrefetchCount = 16;   // opcional
            e.ConfigureConsumer<CompanyCreatedConsumer>(ctx);
            
        });
    });
});

var app = builder.Build();
app.MapGet("/", () => "Audit up");
app.Run();