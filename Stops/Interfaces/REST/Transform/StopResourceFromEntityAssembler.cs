using stops.Domain.Model.Aggregates;
using stops.Interfaces.REST.Resources;

namespace stops.Interfaces.REST.Transform
{
    public static class StopResourceFromEntityAssembler
    {
        /// <summary>
        /// Assembles a StopResource from a Stop. 
        /// </summary>
        /// <param name="entity">The Stop entity</param>
        /// <returns>
        /// A StopResource assembled from the Stop
        /// </returns>
        public static StopResource ToResourceFromEntity(Stop entity) =>
            new StopResource(
                entity.Id,
                entity.Name,
                entity.GoogleMapsUrl,
                entity.ImageUrl,
                entity.Phone,
                entity.FkIdCompany,
                entity.Address,
                entity.Reference,
                entity.FkIdDistrict
            );
    }
}
