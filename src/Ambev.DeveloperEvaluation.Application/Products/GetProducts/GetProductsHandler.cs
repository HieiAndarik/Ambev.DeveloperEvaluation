using Ambev.DeveloperEvaluation.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProducts;

public sealed class GetProductsHandler : IRequestHandler<GetProductsQuery, GetProductsResult>
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public GetProductsHandler(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<GetProductsResult> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var allProducts = await _repository.GetAllAsync(cancellationToken);

        var ordered = request.Order?.ToLower() switch
        {
            "price" => allProducts.OrderBy(p => p.Price),
            "title" => allProducts.OrderBy(p => p.Title),
            _ => allProducts
        };

        var paged = ordered
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .ToList();

        return new GetProductsResult
        {
            Products = _mapper.Map<IEnumerable<ProductDto>>(paged),
            TotalCount = allProducts.Count()
        };
    }
}