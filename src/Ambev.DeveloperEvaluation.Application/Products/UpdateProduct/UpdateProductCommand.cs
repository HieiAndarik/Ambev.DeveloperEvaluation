using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

public sealed class UpdateProductCommand : IRequest<bool>
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public string Category { get; init; } = string.Empty;
    public string Image { get; init; } = string.Empty;
    public double Rate { get; init; }
    public int Count { get; init; }
}