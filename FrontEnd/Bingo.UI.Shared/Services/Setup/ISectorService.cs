using Bingo.Common.DomainModel.Setup;

namespace Bingo.UI.Shared.Services.Setup
{
    public interface ISectorService
    {
        Task<List<Sector>> GetSectorsAsync();
        Task<Sector> GetSectorByIdAsync(Guid Id);
        Task<Sector> NewSectorAsync(Sector request);
        Task<Sector> EditSectorAsync(Sector request);
        Task<bool> DeleteSectorAsync(Guid Id);
    }
}
