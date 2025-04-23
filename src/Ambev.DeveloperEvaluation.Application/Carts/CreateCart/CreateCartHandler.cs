using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart
{
    public sealed class CreateCartHandler : IRequestHandler<CreateCartCommand, int>
    {
        private readonly ICartRepository _cartRepository;

        public CreateCartHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<int> Handle(CreateCartCommand request, CancellationToken cancellationToken)
        {
            var cart = new Cart
            {
                UserId = request.UserId,
                Date = DateTime.SpecifyKind(request.Date, DateTimeKind.Utc),
                Items = new List<CartItem>()
            };

            foreach (var item in request.Items)
            {
                cart.Items.Add(new CartItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                });
            }

            await _cartRepository.AddAsync(cart, cancellationToken);
            return cart.Id;
        }
    }
}
