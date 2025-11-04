using stops.Domain.Model.Aggregates.Geographic;
using stops.Interfaces.REST.Resources.Geographic;

namespace stops.Interfaces.REST.Transform.Geographic
{
    public static class DistrictResourceFromEntityAssembler
    {
        public static DistrictResource ToResourceFromEntity(District entity) =>
            new DistrictResource(
                entity.Id,
                entity.Name,
                entity.FkIdProvince
            );
    }
}
