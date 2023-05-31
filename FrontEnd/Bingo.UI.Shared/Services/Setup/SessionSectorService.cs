using Bingo.Common.DomainModel.Setup;
using System.Net.Http.Json;

namespace Bingo.UI.Shared.Services.Setup
{
    public class SessionSectorService : ISessionSectorService
    {
        private readonly HttpClient _http;

        public SessionSectorService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<SessionSector>> GetSessionSectorsAsync()
        {
            var sessionSectors = await _http.GetFromJsonAsync<List<SessionSector>>($"api/SessionSector/");
            return sessionSectors;
        }

        public async Task<SessionSector> GetSessionSectorByIdAsync(Guid Id)
        {
            var result = await _http.GetAsync($"api/SessionSector/{Id}");
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var message = await result.Content.ReadAsStringAsync();
                var newSector = new SessionSector();
                newSector.Id = Guid.Empty;
                newSector.CreationTimeStamp = DateTime.MinValue;
                newSector.LastUpdateTimeStamp = DateTime.MinValue;

                return newSector;
            }
            else
            {
                return await result.Content.ReadFromJsonAsync<SessionSector>();
            }
        }

        public async Task<SessionSector> NewSessionSectorAsync(SessionSector request)
        {
            var result = await _http.PostAsJsonAsync($"api/SessionSector/", request);
            return await result.Content.ReadFromJsonAsync<SessionSector>();
        }

        public async Task<SessionSector> EditSessionSectorAsync(SessionSector request)
        {
            var result = await _http.PutAsJsonAsync($"api/SessionSector/", request);
            return await result.Content.ReadFromJsonAsync<SessionSector>();
        }

        public async Task<bool> DeleteSessionSectorAsync(Guid Id)
        {
            var result = await _http.DeleteAsync($"api/SessionSector/{Id}");
            return result.IsSuccessStatusCode;
        }
    }
}
