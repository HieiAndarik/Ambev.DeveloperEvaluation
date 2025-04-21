using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProducts;
public sealed record GetProductByIdQuery(int Id) : IRequest<ProductDto?>;
