using Bingo.Common.DomainModel.MasterData;
using System.Net.Http.Json;

namespace Bingo.UI.Shared.Services.MasterData
{
    public class CompetitorCategoryService : ICompetitorCategoryService
    {
        private readonly HttpClient _http;

        public CompetitorCategoryService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<CompetitorCategory>> GetCompetitorCategoriesAsync()
        {
            var competitorCategories = await _http.GetFromJsonAsync<List<CompetitorCategory>>($"api/CompetitorCategory/");
            return competitorCategories;
        }

        public async Task<CompetitorCategory> GetCompetitorCategoryByIdAsync(Guid Id)
        {
            var result = await _http.GetAsync($"api/CompetitorCategory/{Id}");
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var message = await result.Content.ReadAsStringAsync();
                var newCompetitorCategory = new CompetitorCategory();
                newCompetitorCategory.Name = message;
                newCompetitorCategory.Id = Guid.Empty;
                newCompetitorCategory.CreationTimeStamp = DateTime.MinValue;
                newCompetitorCategory.LastUpdateTimeStamp = DateTime.MinValue;

                return newCompetitorCategory;
            }
            else
            {
                return await result.Content.ReadFromJsonAsync<CompetitorCategory>();
            }
        }

        public async Task<CompetitorCategory> NewCompetitorCategoryAsync(CompetitorCategory request)
        {
            var result = await _http.PostAsJsonAsync($"api/CompetitorCategory/", request);
            return await result.Content.ReadFromJsonAsync<CompetitorCategory>();
        }

        public async Task<CompetitorCategory> EditCompetitorCategoryAsync(CompetitorCategory request)
        {
            var result = await _http.PutAsJsonAsync($"api/CompetitorCategory/", request);
            return await result.Content.ReadFromJsonAsync<CompetitorCategory>();
        }

        public async Task<bool> DeleteCompetitorCategoryAsync(Guid Id)
        {
            var result = await _http.DeleteAsync($"api/CompetitorCategory/{Id}");
            return result.IsSuccessStatusCode;
        }
    }
}
