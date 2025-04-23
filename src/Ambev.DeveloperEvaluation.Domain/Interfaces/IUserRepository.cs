using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken);
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<User?> GetByUsernameAndPasswordAsync(string username, string password);
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<User> CreateAsync(User user, CancellationToken cancellationToken = default);
        Task AddAsync(User user);
        Task<bool> UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
