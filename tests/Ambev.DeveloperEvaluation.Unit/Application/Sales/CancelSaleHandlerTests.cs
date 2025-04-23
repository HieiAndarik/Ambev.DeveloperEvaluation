using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using MediatR;
using Moq;
using FluentAssertions;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales
{
    public class CancelSaleHandlerTests
    {
        private readonly Mock<ISaleRepository> _saleRepositoryMock = new();
        private readonly Mock<ISaleService> _saleServiceMock = new();
        private readonly Mock<IMediator> _mediatorMock = new();

        [Fact()]
        public async Task Handle_ShouldCancelSaleSuccessfully()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var sale = new Sale
            {
                Id = saleId,
                Items = new List<SaleItem>
                {
                    new() { Id = Guid.NewGuid(), IsCancelled = false, TotalAmount = 10 },
                    new() { Id = Guid.NewGuid(), IsCancelled = false, TotalAmount = 20 }
                },
                IsCancelled = false,
                TotalAmount = 30
            };

            _saleRepositoryMock.Setup(r => r.GetByIdAsync(saleId)).ReturnsAsync(sale);

            _saleRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Sale>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            _saleServiceMock.Setup(s => s.CancelSale(It.IsAny<Sale>()))
                .Callback<Sale>(s =>
                {
                    s.IsCancelled = true;
                    foreach (var item in s.Items)
                        item.IsCancelled = true;
                    s.TotalAmount = 0;
                });

            var handler = new CancelSaleHandler(_saleRepositoryMock.Object, _saleServiceMock.Object, _mediatorMock.Object);

            // Act
            await handler.Handle(new CancelSaleCommand { Id = saleId }, default);

            // Assert
            sale.IsCancelled.Should().BeTrue();
            sale.Items.All(i => i.IsCancelled).Should().BeTrue();
            sale.TotalAmount.Should().Be(0);
            _saleRepositoryMock.Verify();
        }

        [Fact()]
        public async Task Handle_ShouldCancelSaleItemSuccessfully()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var itemId = Guid.NewGuid();
            var itemToCancel = new SaleItem
            {
                Id = itemId,
                ProductId = "1",
                ProductName = "Item Teste",
                Quantity = 2,
                UnitPrice = 5,
                Discount = 0,
                TotalAmount = 10,
                IsCancelled = false
            };

            var sale = new Sale
            {
                Id = saleId,
                Items = new List<SaleItem>
                {
                    itemToCancel,
                    new() { Id = Guid.NewGuid(), IsCancelled = false, TotalAmount = 20 }
                },
                TotalAmount = 30
            };

            _saleRepositoryMock.Setup(r => r.GetByIdAsync(saleId)).ReturnsAsync(sale);

            _saleRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Sale>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            _saleServiceMock.Setup(s => s.CancelItem(It.IsAny<Sale>(), It.IsAny<string>()))
                .Callback<Sale, string>((s, itemId) =>
                {
                    var item = s.Items.First(i => i.Id.ToString() == itemId);
                    item.IsCancelled = true;
                    s.TotalAmount = s.Items.Where(i => !i.IsCancelled).Sum(i => i.TotalAmount);
                });

            var handler = new CancelSaleItemHandler(_saleRepositoryMock.Object, _saleServiceMock.Object, _mediatorMock.Object);

            // Act
            await handler.Handle(new CancelSaleItemCommand(saleId.ToString(), itemId.ToString()), default);

            // Assert
            itemToCancel.IsCancelled.Should().BeTrue();
            sale.TotalAmount.Should().Be(20);
            _saleRepositoryMock.Verify();
        }

        [Fact()]
        public async Task Handle_ShouldThrow_WhenSaleNotFound()
        {
            // Arrange
            var saleId = Guid.NewGuid();

            _saleRepositoryMock.Setup(r => r.GetByIdAsync(saleId)).ReturnsAsync((Sale)null!);

            var handler = new CancelSaleHandler(_saleRepositoryMock.Object, _saleServiceMock.Object, _mediatorMock.Object);

            // Act
            var act = async () => await handler.Handle(new CancelSaleCommand { Id = saleId }, default);

            // Assert
            await act.Should().ThrowAsync<ValidationException>().WithMessage("*not found*");
        }

        [Fact()]
        public async Task Handle_ShouldThrow_WhenItemNotFoundInSale()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var nonexistentItemId = Guid.NewGuid();

            var sale = new Sale
            {
                Id = saleId,
                Items = new List<SaleItem>
                {
                    new() { Id = Guid.NewGuid(), TotalAmount = 20, IsCancelled = false }
                },
                TotalAmount = 20
            };

            _saleRepositoryMock.Setup(r => r.GetByIdAsync(saleId)).ReturnsAsync(sale);

            _saleServiceMock.Setup(s => s.CancelItem(It.IsAny<Sale>(), nonexistentItemId.ToString()))
                .Callback(() => throw new DomainException("Item not found"));

            var handler = new CancelSaleItemHandler(_saleRepositoryMock.Object, _saleServiceMock.Object, _mediatorMock.Object);

            // Act
            var act = async () => await handler.Handle(new CancelSaleItemCommand(saleId.ToString(), nonexistentItemId.ToString()), default);

            // Assert
            await act.Should().ThrowAsync<DomainException>().WithMessage("*Item not found*");
        }

        [Fact()]
        public async Task Handle_ShouldThrow_WhenSaleAlreadyCancelled()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var sale = new Sale
            {
                Id = saleId,
                IsCancelled = true,
                Items = new List<SaleItem>(),
                TotalAmount = 0
            };

            _saleRepositoryMock.Setup(r => r.GetByIdAsync(saleId)).ReturnsAsync(sale);

            _saleServiceMock.Setup(s => s.CancelSale(sale))
                .Callback(() => throw new DomainException("Sale is already cancelled"));

            var handler = new CancelSaleHandler(_saleRepositoryMock.Object, _saleServiceMock.Object, _mediatorMock.Object);

            // Act
            var act = async () => await handler.Handle(new CancelSaleCommand { Id = saleId }, default);

            // Assert
            await act.Should().ThrowAsync<DomainException>().WithMessage("*already cancelled*");
        }

        [Fact()]
        public async Task Handle_ShouldIgnore_AlreadyCancelledItem()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var itemId = Guid.NewGuid();

            var item = new SaleItem
            {
                Id = itemId,
                IsCancelled = true,
                TotalAmount = 10
            };

            var sale = new Sale
            {
                Id = saleId,
                Items = new List<SaleItem> { item },
                TotalAmount = 10
            };

            _saleRepositoryMock.Setup(r => r.GetByIdAsync(saleId)).ReturnsAsync(sale);

            _saleServiceMock.Setup(s => s.CancelItem(sale, itemId.ToString()))
                .Callback(() =>
                {
                    var item = sale.Items.FirstOrDefault(i => i.Id == itemId);
                    if (item != null && !item.IsCancelled)
                    {
                        item.IsCancelled = true;
                    }

                    sale.TotalAmount = sale.Items
                        .Where(i => !i.IsCancelled)
                        .Sum(i => i.TotalAmount);
                });

            _saleRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Sale>()))
                .Returns(Task.CompletedTask);

            var handler = new CancelSaleItemHandler(_saleRepositoryMock.Object, _saleServiceMock.Object, _mediatorMock.Object);

            // Act
            var act = async () => await handler.Handle(new CancelSaleItemCommand(saleId.ToString(), itemId.ToString()), default);

            // Assert
            await act.Should().NotThrowAsync();
            sale.TotalAmount.Should().Be(0);
        }

        [Fact()]
        public async Task Handle_ShouldCancelSaleWithoutItems()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var sale = new Sale
            {
                Id = saleId,
                Items = new List<SaleItem>(),
                IsCancelled = false,
                TotalAmount = 0
            };

            _saleRepositoryMock.Setup(r => r.GetByIdAsync(saleId)).ReturnsAsync(sale);

            _saleServiceMock.Setup(s => s.CancelSale(sale))
                .Callback(() => { sale.IsCancelled = true; });

            _saleRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Sale>()))
                .Returns(Task.CompletedTask);

            var handler = new CancelSaleHandler(_saleRepositoryMock.Object, _saleServiceMock.Object, _mediatorMock.Object);

            // Act
            await handler.Handle(new CancelSaleCommand { Id = saleId }, default);

            // Assert
            sale.IsCancelled.Should().BeTrue();
            sale.Items.Should().BeEmpty();
        }
    }
}
