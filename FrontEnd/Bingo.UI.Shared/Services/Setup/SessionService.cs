using Bingo.Common.DomainModel.Setup;
using System.Net.Http.Json;

namespace Bingo.UI.Shared.Services.Setup
{
    public class SessionService : ISessionService
    {
        private readonly HttpClient _http;

        public SessionService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Session>> GetSessionsAsync()
        {
            var sessions = await _http.GetFromJsonAsync<List<Session>>($"api/Session/");
            return sessions;
        }

        public async Task<Session> GetSessionByIdAsync(Guid Id)
        {
            var result = await _http.GetAsync($"api/Session/{Id}");
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var message = await result.Content.ReadAsStringAsync();
                var newSession = new Session();
                newSession.Name = message;
                newSession.Id = Guid.Empty;
                newSession.CreationTimeStamp = DateTime.MinValue;
                newSession.LastUpdateTimeStamp = DateTime.MinValue;

                return newSession;
            }
            else
            {
                return await result.Content.ReadFromJsonAsync<Session>();
            }
        }

        public async Task<Session> NewSessionAsync(Session request)
        {
            var result = await _http.PostAsJsonAsync($"api/Session/", request);
            return await result.Content.ReadFromJsonAsync<Session>();
        }

        public async Task<Session> EditSessionAsync(Session request)
        {
            var result = await _http.PutAsJsonAsync($"api/Session/", request);
            return await result.Content.ReadFromJsonAsync<Session>();
        }

        public async Task<bool> DeleteSessionAsync(Guid Id)
        {
            var result = await _http.DeleteAsync($"api/Session/{Id}");
            return result.IsSuccessStatusCode;
        }
    }
}
