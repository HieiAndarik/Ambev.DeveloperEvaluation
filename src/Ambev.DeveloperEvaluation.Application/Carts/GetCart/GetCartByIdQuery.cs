using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCarts;

public sealed record GetCartByIdQuery(int Id) : IRequest<CartDto>;
