namespace Ambev.DeveloperEvaluation.Application.Products.GetProducts;

public sealed class ProductDto
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public string Description { get; init; } = string.Empty;
}