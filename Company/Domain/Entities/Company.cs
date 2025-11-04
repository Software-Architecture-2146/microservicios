using System.ComponentModel.DataAnnotations.Schema;

namespace Company.Api.Domain.Entities;

public class Company
{
    public int Id { get; set; }                  // <- INT (AUTO_INCREMENT en la BD)
    public string Name { get; set; } = "";
    public string? LogoUrl { get; set; }

    public DateTime CreatedAtUtc { get; set; }
    
    [Column("fk_id_user")]
    public int FkIdUser { get; set; }  
}