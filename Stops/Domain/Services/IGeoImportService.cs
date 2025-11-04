using stops.Domain.Model.DTOs;

namespace stops.Domain.Services
{
    public interface IGeoImportService
    {
        Task<IEnumerable<GeoResponseDto>> GetGeoFromApi();
    }
}
