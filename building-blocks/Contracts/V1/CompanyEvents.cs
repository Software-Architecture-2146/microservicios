namespace Contracts.V1;
// EVENTO: Company creada
public record CompanyCreated
{
    public int CompanyId { get; init; }                 // <-- INT (no Guid)
    public string Name { get; init; } = string.Empty;
    public DateTime CreatedAtUtc { get; init; }         // <-- existe (lo usa Audit)
    public CompanyCreated() { }                         // ctor sin params (MassTransit feliz)
}

// (opcionales) si los usas en tu Program.cs:
public record CompanyUpdated
{
    public int CompanyId { get; init; }
    public string Name { get; init; } = string.Empty;
    public DateTime UpdatedAtUtc { get; init; }
    public CompanyUpdated() { }
}

public record CompanyDeleted
{
    public int CompanyId { get; init; }
    public DateTime DeletedAtUtc { get; init; }
    public CompanyDeleted() { }
}

// (opcional) otros eventos, comandos, etc.
// public record CompanyUpdated(...);
// public record DeleteCompany(Guid CompanyId);