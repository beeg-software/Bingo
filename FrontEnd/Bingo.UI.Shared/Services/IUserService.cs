using Bingo.Common.DomainModel.MasterData;

namespace Bingo.UI.Shared.Services
{
    public interface IUserService
    {
        Task<List<User>> GetUsersAsync();
        Task<User> GetUserByIdAsync(Guid Id);
        Task<User> NewUserAsync(User request);
        Task<User> EditUserAsync(User request);
        Task<bool> DeleteUserAsync(Guid Id);
    }
}
