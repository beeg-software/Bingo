using Bingo.Common.DomainModel.Setup;
using Bingo.Common.DomainModel.Timing;
using System.Net.Http.Json;

namespace Bingo.UI.Shared.Services.Timing
{
    public class SectorTimeService : ISectorTimeService
    {
        private readonly HttpClient _http;

        public SectorTimeService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<SectorTime>> GetSectorTimesAsync()
        {
            var sectorTimes = await _http.GetFromJsonAsync<List<SectorTime>>($"api/SectorTime/");
            return sectorTimes;
        }

        public async Task<SectorTime> GetSectorTimeByIdAsync(Guid Id)
        {
            var result = await _http.GetAsync($"api/SectorTime/{Id}");
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var message = await result.Content.ReadAsStringAsync();
                var newSectorTime = new SectorTime();
                newSectorTime.PenaltyNote = message;
                newSectorTime.Id = Guid.Empty;
                newSectorTime.CreationTimeStamp = DateTime.MinValue;
                newSectorTime.LastUpdateTimeStamp = DateTime.MinValue;

                return newSectorTime;
            }
            else
            {
                return await result.Content.ReadFromJsonAsync<SectorTime>();
            }
        }

        public async Task<SectorTime> NewSectorTimeAsync(SectorTime request)
        {
            var result = await _http.PostAsJsonAsync($"api/SectorTime/", request);
            return await result.Content.ReadFromJsonAsync<SectorTime>();
        }

        public async Task<SectorTime> EditSectorTimeAsync(SectorTime request)
        {
            var result = await _http.PutAsJsonAsync($"api/SectorTime/", request);
            return await result.Content.ReadFromJsonAsync<SectorTime>();
        }

        public async Task<bool> DeleteSectorTimeAsync(Guid Id)
        {
            var result = await _http.DeleteAsync($"api/SectorTime/{Id}");
            return result.IsSuccessStatusCode;
        }
    }
}
