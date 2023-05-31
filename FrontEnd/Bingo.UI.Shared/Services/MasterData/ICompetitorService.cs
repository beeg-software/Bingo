using Bingo.Common.DomainModel.MasterData;

namespace Bingo.UI.Shared.Services.MasterData
{
    public interface ICompetitorService
    {
        Task<List<Competitor>> GetCompetitorsAsync();
        Task<Competitor> GetCompetitorByIdAsync(Guid Id);
        Task<Competitor> NewCompetitorAsync(Competitor request);
        Task<Competitor> EditCompetitorAsync(Competitor request);
        Task<bool> DeleteCompetitorAsync(Guid Id);
    }
}
