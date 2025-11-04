namespace stops.Domain.Model.Commands.Geographic
{
    public record CreateDistrictCommand(int Id, string Name, int FkIdProvince);
}
