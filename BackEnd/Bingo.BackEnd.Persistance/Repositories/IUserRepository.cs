using Bingo.Common.DomainModel.MasterData;

namespace Bingo.BackEnd.Persistance.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
        Task AddAsync(User entity);
        Task UpdateAsync(User entity);
        Task DeleteAsync(int id);
    }
}
