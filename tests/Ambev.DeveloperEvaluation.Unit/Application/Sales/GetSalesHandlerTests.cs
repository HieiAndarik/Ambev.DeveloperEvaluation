using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using AutoMapper;
using FluentAssertions;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales
{
    public class GetSalesHandlerTests
    {
        private readonly Mock<ISaleRepository> _saleRepositoryMock = new();
        private readonly IMapper _mapper;

        public GetSalesHandlerTests()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<Ambev.DeveloperEvaluation.WebApi.Mappings.SaleProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task Handle_ShouldReturnPagedSales_WithDefaultOrder()
        {
            // Arrange
            var sales = new List<Sale> { new Sale { SaleNumber = 1, CustomerName = "John Doe" } };
            _saleRepositoryMock.Setup(r => r.GetAllAsync(1, 10, "saleDate desc")).ReturnsAsync(sales);
            _saleRepositoryMock.Setup(r => r.GetTotalCountAsync()).ReturnsAsync(1);

            var handler = new GetSalesHandler(_saleRepositoryMock.Object, _mapper);
            var query = new GetSalesQuery { Page = 1, Size = 10, Order = "saleDate desc" };

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Should().NotBeNull();
            result.TotalCount.Should().Be(1);
            result.Items.Should().HaveCount(1);
        }

        [Fact]
        public async Task Handle_ShouldReturnPagedSales_WithCustomOrder()
        {
            // Arrange
            var sales = new List<Sale> { new Sale { SaleNumber = 2, CustomerName = "Jane Smith" } };
            _saleRepositoryMock.Setup(r => r.GetAllAsync(1, 5, "customerName")).ReturnsAsync(sales);
            _saleRepositoryMock.Setup(r => r.GetTotalCountAsync()).ReturnsAsync(1);

            var handler = new GetSalesHandler(_saleRepositoryMock.Object, _mapper);
            var query = new GetSalesQuery { Page = 1, Size = 5, Order = "customerName" };

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Should().NotBeNull();
            result.TotalCount.Should().Be(1);
            result.Items.Should().HaveCount(1);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoSalesExist()
        {
            // Arrange
            _saleRepositoryMock.Setup(r => r.GetAllAsync(1, 10, "saleDate desc")).ReturnsAsync(new List<Sale>());
            _saleRepositoryMock.Setup(r => r.GetTotalCountAsync()).ReturnsAsync(0);

            var handler = new GetSalesHandler(_saleRepositoryMock.Object, _mapper);
            var query = new GetSalesQuery { Page = 1, Size = 10, Order = "saleDate desc" };

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Should().NotBeNull();
            result.TotalCount.Should().Be(0);
            result.Items.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_ShouldReturnSecondPageCorrectly()
        {
            // Arrange
            var sales = Enumerable.Range(1, 10)
                .Select(i => new Sale { SaleNumber = i, CustomerName = $"Customer {i}" })
                .ToList();

            _saleRepositoryMock.Setup(r => r.GetAllAsync(2, 5, "saleDate desc")).ReturnsAsync(sales.Skip(5).Take(5).ToList());
            _saleRepositoryMock.Setup(r => r.GetTotalCountAsync()).ReturnsAsync(10);

            var handler = new GetSalesHandler(_saleRepositoryMock.Object, _mapper);
            var query = new GetSalesQuery { Page = 2, Size = 5, Order = "saleDate desc" };

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Should().NotBeNull();
            result.TotalCount.Should().Be(10);
            result.Items.Should().HaveCount(5);
        }

        [Fact]
        public async Task Handle_ShouldFallbackToDefaultOrder_WhenInvalidOrderIsProvided()
        {
            // Arrange
            var sales = new List<Sale> { new Sale { SaleNumber = 1, CustomerName = "Fallback Test" } };

            _saleRepositoryMock
                .Setup(r => r.GetAllAsync(1, 10, It.IsAny<string>()))
                .ReturnsAsync(sales);
            _saleRepositoryMock.Setup(r => r.GetTotalCountAsync()).ReturnsAsync(1);

            var handler = new GetSalesHandler(_saleRepositoryMock.Object, _mapper);
            var query = new GetSalesQuery { Page = 1, Size = 10, Order = "invalidField" };

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Should().NotBeNull();
            result.TotalCount.Should().Be(1);
            result.Items.Should().ContainSingle();
        }

        [Fact]
        public async Task Handle_ShouldReturnEmpty_WhenPageOrSizeIsZero()
        {
            // Arrange
            _saleRepositoryMock.Setup(r => r.GetAllAsync(0, 0, "saleDate desc")).ReturnsAsync(new List<Sale>());
            _saleRepositoryMock.Setup(r => r.GetTotalCountAsync()).ReturnsAsync(0);

            var handler = new GetSalesHandler(_saleRepositoryMock.Object, _mapper);
            var query = new GetSalesQuery { Page = 0, Size = 0, Order = "saleDate desc" };

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Should().NotBeNull();
            result.TotalCount.Should().Be(0);
            result.Items.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_ShouldReturnInconsistentResult_WhenTotalCountMismatch()
        {
            // Arrange
            var sales = new List<Sale>(); // nenhum item, mas TotalCount = 5
            _saleRepositoryMock.Setup(r => r.GetAllAsync(1, 10, "saleDate desc")).ReturnsAsync(sales);
            _saleRepositoryMock.Setup(r => r.GetTotalCountAsync()).ReturnsAsync(5);

            var handler = new GetSalesHandler(_saleRepositoryMock.Object, _mapper);
            var query = new GetSalesQuery { Page = 1, Size = 10 };

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Should().NotBeNull();
            result.TotalCount.Should().Be(5);
            result.Items.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_ShouldMapSaleWithNullProperties()
        {
            // Arrange
            var sales = new List<Sale> {
                new Sale {
                    Id = Guid.NewGuid(),
                    CustomerName = null!,
                    BranchName = null!,
                    Items = new List<SaleItem>()
                }};

            _saleRepositoryMock.Setup(r => r.GetAllAsync(1, 10, "saleDate desc")).ReturnsAsync(sales);
            _saleRepositoryMock.Setup(r => r.GetTotalCountAsync()).ReturnsAsync(1);

            var handler = new GetSalesHandler(_saleRepositoryMock.Object, _mapper);
            var query = new GetSalesQuery { Page = 1, Size = 10 };

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(1);
        }
    }
}