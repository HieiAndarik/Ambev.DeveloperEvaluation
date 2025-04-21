using Ambev.DeveloperEvaluation.Domain.Interfaces;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

public sealed class UpdateProductHandler : IRequestHandler<UpdateProductCommand, bool>
{
    private readonly IProductRepository _repository;

    public UpdateProductHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (product is null) return false;

        product.Title = request.Title;
        product.Description = request.Description;
        product.Price = request.Price;
        product.Category = request.Category;
        product.Image = request.Image;
        product.Rate = request.Rate;
        product.Count = request.Count;

        await _repository.UpdateAsync(product, cancellationToken);
        return true;
    }
}