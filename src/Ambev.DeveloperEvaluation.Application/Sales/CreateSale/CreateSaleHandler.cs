using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        // Aqui poderia ter um IBranchRepository se existisse

        public CreateSaleHandler(
            ISaleRepository saleRepository,
            IUserRepository userRepository,
            IProductRepository productRepository)
        {
            _saleRepository = saleRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
        }

        public async Task<CreateSaleResult> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
        {
            // Obter dados do cliente
            var customer = await _userRepository.GetByIdAsync(request.CustomerId);
            if (customer == null)
                throw new ValidationException("Customer not found");

            // Aqui poderia buscar os dados do Branch, mas como não temos essa entidade
            // no projeto, vou usar valores fictícios
            string branchName = "Main Branch"; // Em um cenário real, isso viria do banco

            // Obter próximo número de venda
            int saleNumber = await _saleRepository.GetNextSaleNumberAsync();

            // Criar a venda
            var sale = Sale.Create(
                saleNumber,
                customer.Id,
                $"{customer.Name.Firstname} {customer.Name.Lastname}",
                request.BranchId,
                branchName);

            // Adicionar itens
            foreach (var itemDto in request.Items)
            {
                var product = await _productRepository.GetByIdAsync(itemDto.ProductId);
                if (product == null)
                    throw new ValidationException($"Product with ID {itemDto.ProductId} not found");

                sale.AddItem(
                    product.Id,
                    product.Title,
                    itemDto.Quantity,
                    product.Price);
            }

            // Persistir a venda
            await _saleRepository.AddAsync(sale);

            // Publicar evento de SaleCreated (opcional)
            // _mediator.Publish(new SaleCreatedEvent(sale.Id));

            return new CreateSaleResult
            {
                Id = sale.Id,
                SaleNumber = sale.SaleNumber,
                TotalAmount = sale.TotalAmount
            };
        }
    }
}