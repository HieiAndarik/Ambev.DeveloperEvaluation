using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken);
        Task<Product?> GetByIdAsync(int id);
        Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task AddAsync(Product product);
        Task AddAsync(Product product, CancellationToken cancellationToken);
        Task UpdateAsync(Product product);
        Task UpdateAsync(Product product, CancellationToken cancellationToken);
        Task DeleteAsync(Product product);
        Task<bool> DeleteByIdAsync(int id);
        Task<bool> DeleteByIdAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<string>> GetAllCategoriesAsync();
        Task<IEnumerable<Product>> GetByCategoryAsync(string category);
        Task<IEnumerable<Product>> GetByCategoryAsync(string category, CancellationToken cancellationToken);
    }
}
