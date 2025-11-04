namespace stops.Domain.Model.Commands.Geographic
{
    public record CreateProvinceCommand(int Id, string Name, int FkIdRegion);
}
