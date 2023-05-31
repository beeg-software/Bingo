using Bingo.Common.DomainModel.Timing;

namespace Bingo.UI.Shared.Services.Timing
{
    public interface ISectorTimeService
    {
        Task<List<SectorTime>> GetSectorTimesAsync();
        Task<SectorTime> GetSectorTimeByIdAsync(Guid Id);
        Task<SectorTime> NewSectorTimeAsync(SectorTime request);
        Task<SectorTime> EditSectorTimeAsync(SectorTime request);
        Task<bool> DeleteSectorTimeAsync(Guid Id);
    }
}
