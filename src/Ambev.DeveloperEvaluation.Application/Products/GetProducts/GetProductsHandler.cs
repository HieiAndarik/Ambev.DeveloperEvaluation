using Ambev.DeveloperEvaluation.Domain.Interfaces;
using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Common.Utils;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProducts;

public sealed class GetProductsHandler : IRequestHandler<GetProductsQuery, ProductsListResponse<ProductDto>>
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public GetProductsHandler(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ProductsListResponse<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var allProducts = await _repository.GetAllAsync(cancellationToken);
        var ordered = allProducts.AsEnumerable().ApplyOrdering(request.Order);

        var totalItems = ordered.Count();
        var paged = ordered.ApplyPagination(request.Page, request.Size).ToList();

        var mapped = _mapper.Map<IEnumerable<ProductDto>>(paged);

        return new ProductsListResponse<ProductDto>
        {
            Data = mapped,
            TotalItems = totalItems,
            CurrentPage = request.Page,
            TotalPages = (int)Math.Ceiling((double)totalItems / request.Size)
        };
    }
}
