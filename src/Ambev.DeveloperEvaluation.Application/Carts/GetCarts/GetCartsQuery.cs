using Ambev.DeveloperEvaluation.Application.Carts.GetCartById;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCarts;

public sealed record GetCartsQuery(int Page = 1, int Size = 10, string? Order = null) : IRequest<GetCartsResult>;