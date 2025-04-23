using Ambev.DeveloperEvaluation.Domain.Interfaces;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;

public sealed class DeleteCartHandler : IRequestHandler<DeleteCartCommand, bool>
{
    private readonly ICartRepository _repository;

    public DeleteCartHandler(ICartRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteCartCommand request, CancellationToken cancellationToken)
    {
        return await _repository.DeleteByIdAsync(request.Id, cancellationToken);
    }
}
