using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Services;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly ISaleService _saleService;
        private readonly IMediator _mediator;

        public CreateSaleHandler(
            ISaleRepository saleRepository,
            IUserRepository userRepository,
            IProductRepository productRepository,
            ISaleService saleService,
            IMediator mediator)
        {
            _saleRepository = saleRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
            _saleService = saleService;
            _mediator = mediator;
        }

        public async Task<CreateSaleResult> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
        {
            // Get customer data
            var customer = await _userRepository.GetByIdAsync(request.CustomerId);
            if (customer == null)
                throw new ValidationException("Customer not found");

            // In a real scenario, we'd get branch data from a repository
            string branchName = "Main Branch"; // Example value

            // Get next sale number
            int saleNumber = await _saleRepository.GetNextSaleNumberAsync();

            // Create sale
            var sale = _saleService.CreateSale(
                saleNumber,
                request.CustomerId,
                $"{customer.Name.Firstname} {customer.Name.Lastname}",
                request.BranchId,
                branchName);

            // Add items
            foreach (var itemDto in request.Items)
            {
                var product = await _productRepository.GetByIdAsync(itemDto.ProductId);
                if (product == null)
                    throw new ValidationException($"Product with ID {itemDto.ProductId} not found");

                _saleService.AddItem(
                    sale,
                    product.Id.ToString(),
                    product.Title,
                    itemDto.Quantity,
                    product.Price);
            }

            // Save sale
            await _saleRepository.AddAsync(sale);

            // Publish event
            await _mediator.Publish(new SaleCreatedEvent(sale.Id), cancellationToken);

            return new CreateSaleResult
            {
                Id = sale.Id,
                SaleNumber = sale.SaleNumber,
                TotalAmount = sale.TotalAmount
            };
        }
    }
}