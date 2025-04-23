using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Interfaces
{
    public interface ICartRepository
    {
        Task<IEnumerable<Cart>> GetAllAsync();
        Task<IEnumerable<Cart>> GetAllAsync(CancellationToken cancellationToken);
        Task<Cart?> GetByIdAsync(int id);
        Task<Cart?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task AddAsync(Cart cart);
        Task AddAsync(Cart cart, CancellationToken cancellationToken);
        Task UpdateAsync(Cart cart);
        Task UpdateAsync(Cart cart, CancellationToken cancellationToken);
        Task DeleteAsync(Cart cart);
        Task DeleteAsync(Cart cart, CancellationToken cancellationToken);
        Task<bool> DeleteByIdAsync(int id);
        Task<bool> DeleteByIdAsync(int id, CancellationToken cancellationToken);
        Task RemoveItemsAsync(IEnumerable<CartItem> items, CancellationToken cancellationToken);

    }
}
