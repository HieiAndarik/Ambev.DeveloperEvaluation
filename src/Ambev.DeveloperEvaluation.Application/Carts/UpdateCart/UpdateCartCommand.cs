using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart
{
    public sealed class UpdateCartCommand : IRequest<bool>
    {
        public int Id { get; init; }
        public Guid UserId { get; init; }
        public DateTime Date { get; init; }
    }
}