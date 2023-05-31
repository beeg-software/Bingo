using Bingo.Common.DomainModel.MasterData;
using System.Net.Http.Json;

namespace Bingo.UI.Shared.Services.MasterData
{
    public class CompetitorService : ICompetitorService
    {
        private readonly HttpClient _http;

        public CompetitorService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Competitor>> GetCompetitorsAsync()
        {
            var competitors = await _http.GetFromJsonAsync<List<Competitor>>($"api/Competitor/");
            return competitors;
        }

        public async Task<Competitor> GetCompetitorByIdAsync(Guid Id)
        {
            var result = await _http.GetAsync($"api/Competitor/{Id}");
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var message = await result.Content.ReadAsStringAsync();
                var newCompetitor = new Competitor();
                newCompetitor.Name1 = message;
                newCompetitor.Id = Guid.Empty;
                newCompetitor.CreationTimeStamp = DateTime.MinValue;
                newCompetitor.LastUpdateTimeStamp = DateTime.MinValue;

                return newCompetitor;
            }
            else
            {
                return await result.Content.ReadFromJsonAsync<Competitor>();
            }
        }

        public async Task<Competitor> NewCompetitorAsync(Competitor request)
        {
            var result = await _http.PostAsJsonAsync($"api/Competitor/", request);
            return await result.Content.ReadFromJsonAsync<Competitor>();
        }

        public async Task<Competitor> EditCompetitorAsync(Competitor request)
        {
            var result = await _http.PutAsJsonAsync($"api/Competitor/", request);
            return await result.Content.ReadFromJsonAsync<Competitor>();
        }

        public async Task<bool> DeleteCompetitorAsync(Guid Id)
        {
            var result = await _http.DeleteAsync($"api/Competitor/{Id}");
            return result.IsSuccessStatusCode;
        }
    }
}
