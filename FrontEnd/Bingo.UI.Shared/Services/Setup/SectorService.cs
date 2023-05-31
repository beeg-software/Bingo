using Bingo.Common.DomainModel.Setup;
using System.Net.Http.Json;

namespace Bingo.UI.Shared.Services.Setup
{
    public class SectorService : ISectorService
    {
        private readonly HttpClient _http;

        public SectorService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Sector>> GetSectorsAsync()
        {
            var sectors = await _http.GetFromJsonAsync<List<Sector>>($"api/Sector/");
            return sectors;
        }

        public async Task<Sector> GetSectorByIdAsync(Guid Id)
        {
            var result = await _http.GetAsync($"api/Sector/{Id}");
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var message = await result.Content.ReadAsStringAsync();
                var newSector = new Sector();
                newSector.Name = message;
                newSector.Id = Guid.Empty;
                newSector.CreationTimeStamp = DateTime.MinValue;
                newSector.LastUpdateTimeStamp = DateTime.MinValue;

                return newSector;
            }
            else
            {
                return await result.Content.ReadFromJsonAsync<Sector>();
            }
        }

        public async Task<Sector> NewSectorAsync(Sector request)
        {
            var result = await _http.PostAsJsonAsync($"api/Sector/", request);
            return await result.Content.ReadFromJsonAsync<Sector>();
        }

        public async Task<Sector> EditSectorAsync(Sector request)
        {
            var result = await _http.PutAsJsonAsync($"api/Sector/", request);
            return await result.Content.ReadFromJsonAsync<Sector>();
        }

        public async Task<bool> DeleteSectorAsync(Guid Id)
        {
            var result = await _http.DeleteAsync($"api/Sector/{Id}");
            return result.IsSuccessStatusCode;
        }
    }
}
