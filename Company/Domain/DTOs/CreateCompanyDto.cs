namespace Company.Domain.DTOs;

public class CreateCompanyDto
{
    public string Name { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    public int FkIdUser { get; set; }
}

public class CompanyDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string? LogoUrl { get; set; }
    public int FkIdUser { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}
