using Bingo.Common.DomainModel.Setup;

namespace Bingo.UI.Shared.Services.Setup
{
    public interface ISessionSectorService
    {
        Task<List<SessionSector>> GetSessionSectorsAsync();
        Task<SessionSector> GetSessionSectorByIdAsync(Guid Id);
        Task<SessionSector> NewSessionSectorAsync(SessionSector request);
        Task<SessionSector> EditSessionSectorAsync(SessionSector request);
        Task<bool> DeleteSessionSectorAsync(Guid Id);
    }
}
