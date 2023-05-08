using Blank7.Common.DomainModel.MasterData;

namespace Blank7.UI.Shared.Services
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
