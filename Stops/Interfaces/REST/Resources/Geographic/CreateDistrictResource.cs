namespace stops.Interfaces.REST.Resources.Geographic
{
    public record CreateDistrictResource(
        int Id,
        string Name,
        int FkIdProvince
        );
}
