using Bingo.Common.DomainModel.Setup;

namespace Bingo.UI.Shared.Services.Setup
{
    public interface ISessionService
    {
        Task<List<Session>> GetSessionsAsync();
        Task<Session> GetSessionByIdAsync(Guid Id);
        Task<Session> NewSessionAsync(Session request);
        Task<Session> EditSessionAsync(Session request);
        Task<bool> DeleteSessionAsync(Guid Id);
    }
}
