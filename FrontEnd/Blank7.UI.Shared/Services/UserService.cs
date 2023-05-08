using Blank7.Common.DomainModel.MasterData;
using System.Net.Http.Json;

namespace Blank7.UI.Shared.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _http;

        public UserService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            var users = await _http.GetFromJsonAsync<List<User>>($"api/User/");
            return users;
        }

        public async Task<User> GetUserByIdAsync(Guid Id)
        {
            var result = await _http.GetAsync($"api/User/{Id}");
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var message = await result.Content.ReadAsStringAsync();
                return new User(Guid.NewGuid(), message, DateTime.UtcNow);
            }
            else
            {
                return await result.Content.ReadFromJsonAsync<User>();
            }
        }

        public async Task<User> NewUserAsync(User request)
        {
            var result = await _http.PostAsJsonAsync($"api/User/", request);
            return await result.Content.ReadFromJsonAsync<User>();
        }

        public async Task<User> EditUserAsync(User request)
        {
            var result = await _http.PutAsJsonAsync($"api/User/", request);
            return await result.Content.ReadFromJsonAsync<User>();
        }

        public async Task<bool> DeleteUserAsync(Guid Id)
        {
            var result = await _http.DeleteAsync($"api/User/{Id}");
            return result.IsSuccessStatusCode;
        }
    }
}
