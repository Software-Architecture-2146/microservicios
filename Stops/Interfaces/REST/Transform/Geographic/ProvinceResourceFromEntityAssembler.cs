using stops.Domain.Model.Aggregates.Geographic;
using stops.Interfaces.REST.Resources.Geographic;

namespace stops.Interfaces.REST.Transform.Geographic
{
    public static class ProvinceResourceFromEntityAssembler
    {
        public static ProvinceResource ToResourceFromEntity(Province entity) =>
            new ProvinceResource(
                entity.Id,
                entity.Name,
                entity.FkIdRegion
            );
    }
}
