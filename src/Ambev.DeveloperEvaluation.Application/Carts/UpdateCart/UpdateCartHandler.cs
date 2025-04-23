using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public sealed class UpdateCartHandler : IRequestHandler<UpdateCartCommand, bool>
{
    private readonly ICartRepository _cartRepository;

    public UpdateCartHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<bool> Handle(UpdateCartCommand request, CancellationToken cancellationToken)
    {
        var existingCart = await _cartRepository.GetByIdAsync(request.Id, cancellationToken);

        if (existingCart is null)
            return false;

        existingCart.UserId = request.UserId;
        existingCart.Date = DateTime.SpecifyKind(request.Date, DateTimeKind.Utc);

        await _cartRepository.RemoveItemsAsync(existingCart.Items, cancellationToken);

        foreach (var item in request.Items)
        {
            existingCart.Items.Add(new CartItem
            {
                CartId = existingCart.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity
            });
        }

        await _cartRepository.UpdateAsync(existingCart, cancellationToken);
        return true;
    }

}
