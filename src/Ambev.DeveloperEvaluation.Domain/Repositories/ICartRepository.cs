using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface ICartRepository
{
    Task<IEnumerable<Cart>> GetAllAsync(CancellationToken cancellationToken);
}