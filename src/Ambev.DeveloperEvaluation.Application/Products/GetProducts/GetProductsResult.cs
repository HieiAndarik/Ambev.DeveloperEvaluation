using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProducts;

public sealed class GetProductsResult
{
    public IEnumerable<ProductDto> Products { get; init; } = new List<ProductDto>();
    public int TotalCount { get; init; }
}