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
    }
}
