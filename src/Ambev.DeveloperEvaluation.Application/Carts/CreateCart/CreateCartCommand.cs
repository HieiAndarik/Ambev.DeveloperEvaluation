using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart
{
    public sealed class CreateCartCommand : IRequest<int>
    {
        public Guid UserId { get; init; }
        public DateTime Date { get; init; }
        public List<CreateCartItemDto> Items { get; init; } = new();
    }
}
