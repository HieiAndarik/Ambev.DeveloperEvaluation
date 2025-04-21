using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;

public sealed record DeleteProductCommand(int Id) : IRequest<bool>;