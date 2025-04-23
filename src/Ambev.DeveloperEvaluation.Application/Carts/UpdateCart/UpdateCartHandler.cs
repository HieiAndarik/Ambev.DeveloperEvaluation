using Ambev.DeveloperEvaluation.Domain.Interfaces;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart
{
    public sealed class UpdateCartHandler : IRequestHandler<UpdateCartCommand, bool>
    {
        private readonly ICartRepository _cartRepository;

        public UpdateCartHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<bool> Handle(UpdateCartCommand command, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetByIdAsync(command.Id, cancellationToken);
            if (cart is null)
                return false;

            cart.UserId = command.UserId;
            cart.Date = command.Date;

            return await _cartRepository.UpdateAsync(cart, cancellationToken);
        }
    }
}