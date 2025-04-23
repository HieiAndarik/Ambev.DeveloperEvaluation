using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;

public sealed record DeleteCartCommand(int Id) : IRequest<bool>;
