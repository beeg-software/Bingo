using Blank7.Common.DomainModel.MasterData;

namespace Blank7.UI.Shared.Services
{
    public interface IUserService
    {
        Task<List<User>> GetUsersAsync();
    }
}
