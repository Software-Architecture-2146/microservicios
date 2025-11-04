using stops.Domain.Model.Aggregates.Geographic;
using stops.Interfaces.REST.Resources.Geographic;

namespace stops.Interfaces.REST.Transform.Geographic
{
    public static class RegionResourceFromEntityAssembler
    {
        public static RegionResource ToResourceFromEntity(Region entity) =>
            new RegionResource(
                entity.Id,
                entity.Name
            );
    }
}
