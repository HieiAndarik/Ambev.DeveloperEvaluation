using Ambev.DeveloperEvaluation.Application.Carts.GetCarts;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart
{
    public sealed class UpdateCartCommand : IRequest<bool>
    {
        public int Id { get; init; }
        public Guid UserId { get; init; }
        public DateTime Date { get; init; }
        public List<UpdateCartItemDto> Items { get; init; } = new();
    }
}