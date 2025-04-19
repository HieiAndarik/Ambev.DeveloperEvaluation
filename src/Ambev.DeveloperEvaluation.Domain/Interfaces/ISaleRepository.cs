using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Interfaces
{
    public interface ISaleRepository
    {
        Task<Sale> GetByIdAsync(int id);
        Task<IEnumerable<Sale>> GetAllAsync(int page, int size, string orderBy);
        Task<int> AddAsync(Sale sale);
        Task UpdateAsync(Sale sale);
        Task DeleteAsync(int id);
        Task<int> GetNextSaleNumberAsync();
    }
}