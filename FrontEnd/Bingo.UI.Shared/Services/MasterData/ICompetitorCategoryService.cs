using Bingo.Common.DomainModel.MasterData;

namespace Bingo.UI.Shared.Services.MasterData
{
    public interface ICompetitorCategoryService
    {
        Task<List<CompetitorCategory>> GetCompetitorCategoriesAsync();
        Task<CompetitorCategory> GetCompetitorCategoryByIdAsync(Guid Id);
        Task<CompetitorCategory> NewCompetitorCategoryAsync(CompetitorCategory request);
        Task<CompetitorCategory> EditCompetitorCategoryAsync(CompetitorCategory request);
        Task<bool> DeleteCompetitorCategoryAsync(Guid Id);
    }
}
