using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using Ambev.DeveloperEvaluation.WebApi.Mappings;
using AutoMapper;
using FluentAssertions;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales
{
    public class GetSaleHandlerTests
    {
        private readonly Mock<ISaleRepository> _saleRepositoryMock = new();
        private readonly IMapper _mapper;

        public GetSaleHandlerTests()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<SaleProfile>();
                cfg.AddProfile<ProductProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task Handle_ShouldReturnSaleDetail_WhenIdIsValid()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var sale = new Sale
            {
                Id = saleId,
                CustomerName = "Test Customer",
                BranchName = "Test Branch",
                SaleDate = DateTime.UtcNow,
                TotalAmount = 100,
                IsCancelled = false,
                Items = new List<SaleItem>
                {
                    new SaleItem
                    {
                        Id = Guid.NewGuid(),
                        ProductName = "Item 1",
                        Quantity = 2,
                        UnitPrice = 10,
                        Discount = 0.1m,
                        TotalAmount = 18,
                        IsCancelled = false
                    }
                }
            };

            _saleRepositoryMock.Setup(r => r.GetByIdAsync(saleId)).ReturnsAsync(sale);

            var handler = new GetSaleHandler(_saleRepositoryMock.Object, _mapper);
            var query = new GetSaleQuery { Id = saleId };

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Should().NotBeNull();
            result.CustomerName.Should().Be("Test Customer");
            result.Items.Should().HaveCount(1);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenSaleNotFound()
        {
            // Arrange
            var query = new GetSaleQuery { Id = Guid.NewGuid() };
            _saleRepositoryMock.Setup(r => r.GetByIdAsync(query.Id)).ReturnsAsync((Sale?)null);

            var handler = new GetSaleHandler(_saleRepositoryMock.Object, _mapper);

            // Act
            Func<Task> act = async () => await handler.Handle(query, default);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Sale not found");
        }

        [Fact]
        public async Task Handle_ShouldReturnSale_WhenNoItemsExist()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var sale = new Sale
            {
                Id = saleId,
                CustomerName = "Cliente Sem Itens",
                BranchName = "Filial Teste",
                SaleDate = DateTime.UtcNow,
                TotalAmount = 0,
                IsCancelled = false,
                Items = new List<SaleItem>() // vazio
            };

            _saleRepositoryMock.Setup(r => r.GetByIdAsync(saleId)).ReturnsAsync(sale);

            var handler = new GetSaleHandler(_saleRepositoryMock.Object, _mapper);
            var query = new GetSaleQuery { Id = saleId };

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_ShouldReturnSale_WithCancelledItems()
        {
            var saleId = Guid.NewGuid();
            var sale = new Sale
            {
                Id = saleId,
                CustomerName = "Cliente com Itens Cancelados",
                BranchName = "Filial Z",
                SaleDate = DateTime.UtcNow,
                TotalAmount = 90,
                IsCancelled = false,
                Items = new List<SaleItem>
                    {
                        new SaleItem { Id = Guid.NewGuid(), ProductName = "Produto A", Quantity = 1, UnitPrice = 100, Discount = 0, TotalAmount = 100, IsCancelled = true },
                        new SaleItem { Id = Guid.NewGuid(), ProductName = "Produto B", Quantity = 1, UnitPrice = 90, Discount = 0, TotalAmount = 90, IsCancelled = false },
                    }
            };

            _saleRepositoryMock.Setup(r => r.GetByIdAsync(saleId)).ReturnsAsync(sale);

            var handler = new GetSaleHandler(_saleRepositoryMock.Object, _mapper);
            var query = new GetSaleQuery { Id = saleId };

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(2);
            result.Items.Should().Contain(i => i.IsCancelled);
        }
    }
}