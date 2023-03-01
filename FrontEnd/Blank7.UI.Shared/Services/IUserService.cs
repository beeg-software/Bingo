namespace Blank7.UI.Shared.Services
{
    public interface IUserService
    {
        Task<List<string>> GetUsers();
    }
}
