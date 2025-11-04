using Contracts.V1;
using MassTransit;

namespace Audit.Api.Consumers;

public class CompanyCreatedConsumer : IConsumer<CompanyCreated>
{
    private readonly ILogger<CompanyCreatedConsumer> _log;
    public CompanyCreatedConsumer(ILogger<CompanyCreatedConsumer> log) => _log = log;

    public Task Consume(ConsumeContext<CompanyCreated> ctx)
    {
        var m = ctx.Message;
        _log.LogInformation("AUDIT -> CompanyCreated Id={Id} Name={Name} At={At}",
            m.CompanyId, m.Name, m.CreatedAtUtc);
        // TODO: persistir audit si aplica
        return Task.CompletedTask;
    }
}