using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProducts;
public sealed record GetProductsByCategoryQuery(string Category, int Page = 1, int Size = 10, string? Order = null)
    : IRequest<ProductsListResponse<ProductDto>>;
