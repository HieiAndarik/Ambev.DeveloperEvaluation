using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using AutoMapper;
using FluentAssertions;
using Moq;
using Xunit;
using MediatR;
using Ambev.DeveloperEvaluation.WebApi.Mappings;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Tests.Sales
{
    public class CreateSaleValidatorTests
    {
        private readonly CreateSaleValidator _validator = new();
        private readonly Mock<IUserRepository> _userRepositoryMock = new();
        private readonly Mock<IProductRepository> _productRepositoryMock = new();
        private readonly Mock<ISaleRepository> _saleRepositoryMock = new();
        private readonly Mock<ISaleService> _saleServiceMock = new();
        private readonly Mock<IMediator> _mediatorMock = new();
        private readonly IMapper _mapper;

        public CreateSaleValidatorTests()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<SaleProfile>();
                cfg.AddProfile<ProductProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public void Should_Fail_When_CustomerId_Is_Empty()
        {
            var command = new CreateSaleCommand
            {
                CustomerId = Guid.Empty,
                BranchId = Guid.NewGuid(),
                Items = new List<CreateSaleItemDto> { new() { ProductId = 101, Quantity = 1 } }
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "CustomerId");
        }

        [Fact]
        public void Should_Fail_When_BranchId_Is_Empty()
        {
            var command = new CreateSaleCommand
            {
                CustomerId = Guid.NewGuid(),
                BranchId = Guid.Empty,
                Items = new List<CreateSaleItemDto> { new() { ProductId = 101, Quantity = 1 } }
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "BranchId");
        }

        [Fact]
        public void Should_Fail_When_Items_Is_Empty()
        {
            var command = new CreateSaleCommand
            {
                CustomerId = Guid.NewGuid(),
                BranchId = Guid.NewGuid(),
                Items = new List<CreateSaleItemDto>()
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Items");
        }

        [Fact]
        public void Should_Fail_When_Item_Quantity_Is_Invalid()
        {
            var command = new CreateSaleCommand
            {
                CustomerId = Guid.NewGuid(),
                BranchId = Guid.NewGuid(),
                Items = new List<CreateSaleItemDto>
                {
                    new() { ProductId = 101, Quantity = 0 },
                    new() { ProductId = 102, Quantity = 21 }
                }
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Items[0].Quantity");
            result.Errors.Should().Contain(e => e.PropertyName == "Items[1].Quantity");
        }

        [Fact]
        public void Should_Pass_With_Valid_Data()
        {
            var command = new CreateSaleCommand
            {
                CustomerId = Guid.NewGuid(),
                BranchId = Guid.NewGuid(),
                Items = new List<CreateSaleItemDto>
                {
                    new() { ProductId = 101, Quantity = 5 },
                    new() { ProductId = 102, Quantity = 1 }
                }
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Should_Pass_With_10_Percent_Discount()
        {
            var command = new CreateSaleCommand
            {
                CustomerId = Guid.NewGuid(),
                BranchId = Guid.NewGuid(),
                Items = new List<CreateSaleItemDto>
                {
                    new() { ProductId = 101, Quantity = 5 }
                }
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Should_Pass_With_20_Percent_Discount()
        {
            var command = new CreateSaleCommand
            {
                CustomerId = Guid.NewGuid(),
                BranchId = Guid.NewGuid(),
                Items = new List<CreateSaleItemDto>
                {
                    new() { ProductId = 201, Quantity = 15 }
                }
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Should_Fail_When_Quantity_Exceeds_20()
        {
            var command = new CreateSaleCommand
            {
                CustomerId = Guid.NewGuid(),
                BranchId = Guid.NewGuid(),
                Items = new List<CreateSaleItemDto>
                {
                    new() { ProductId = 999, Quantity = 25 }
                }
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Items[0].Quantity");
        }

        [Fact]
        public async Task Handle_ShouldCreateSale_With20PercentDiscount()
        {
            // Arrange
            var customerId = Guid.Parse("eea3ebf9-2ff6-4670-81ae-fcc22477ef96");
            var branchId = Guid.NewGuid();
            var productId = 2;
            decimal unitPrice = 5;

            var command = new CreateSaleCommand
            {
                CustomerId = customerId,
                BranchId = branchId,
                Items = new List<CreateSaleItemDto>
                {
                    new() { ProductId = productId, Quantity = 15 }
                }
            };

            var user = new User
            {
                Id = customerId,
                FirstName = "Test",
                LastName = "User",
                Email = "teste@ambev.com",
                Password = "Teste123",
                Role = Domain.Enums.UserRole.Customer,
                Status = Domain.Enums.UserStatus.Active
            };

            var sale = new Sale
            {
                Id = Guid.NewGuid(),
                Items = new List<SaleItem>(),
                TotalAmount = 0
            };

            var product = new Product { Id = productId, Title = "Item A", Price = unitPrice };

            _userRepositoryMock
                .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(user);

            _productRepositoryMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(product);

            _saleServiceMock.Setup(s => s.CreateSale(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new Sale { Items = new List<SaleItem>(), TotalAmount = 0 });

            _saleServiceMock
                .Setup(s => s.CreateSale(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(sale);

            _saleServiceMock
                .Setup(s => s.AddItem(It.IsAny<Sale>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<decimal>()))
                .Callback<Sale, string, string, int, decimal>((s, pid, pname, qty, price) =>
                {
                    var discount = 0.2m;
                    var itemTotal = qty * price * (1 - discount);

                    s.Items.Add(new SaleItem
                    {
                        Id = Guid.NewGuid(),
                        ProductId = pid,
                        ProductName = pname,
                        Quantity = qty,
                        UnitPrice = price,
                        Discount = discount,
                        TotalAmount = itemTotal
                    });

                s.TotalAmount += itemTotal;
                });

            var handler = new CreateSaleHandler(
                _saleRepositoryMock.Object,
                _userRepositoryMock.Object,
                _productRepositoryMock.Object,
                _saleServiceMock.Object,
                _mediatorMock.Object);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().NotBeEmpty();
            result.TotalAmount.Should().Be(60m);
        }

        [Fact()]
        public async Task Handle_ShouldCreateSale_With10PercentDiscount()
        {
            // Arrange
            var customerId = Guid.Parse("eea3ebf9-2ff6-4670-81ae-fcc22477ef96");
            var branchId = Guid.NewGuid();
            var productId = 2;

            var command = new CreateSaleCommand
            {
                CustomerId = customerId,
                BranchId = branchId,
                Items = new List<CreateSaleItemDto>
                    {
                        new() { ProductId = productId, Quantity = 10 }
                    }
            };

            var user = new User
            {
                Id = customerId,
                FirstName = "Test",
                LastName = "User",
                Email = "teste@ambev.com",
                Password = "Teste123",
                Role = Domain.Enums.UserRole.Customer,
                Status = Domain.Enums.UserStatus.Active
            };

            var product = new Product { Id = productId, Title = "Item A", Price = 10.0m };
            var sale = new Sale { Id = Guid.NewGuid(), Items = new List<SaleItem>(), TotalAmount = 0 };

            _userRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);
            _productRepositoryMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(product);

            _saleServiceMock.Setup(s => s.CreateSale(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(sale);

            _saleServiceMock.Setup(s => s.AddItem(It.IsAny<Sale>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<decimal>()))
                .Callback<Sale, string, string, int, decimal>((s, pid, pname, qty, price) =>
                {
                    var itemTotal = qty * price * 0.9m;
                    s.Items.Add(new SaleItem
                    {
                        Id = Guid.NewGuid(),
                        ProductId = pid,
                        ProductName = pname,
                        Quantity = qty,
                        UnitPrice = price,
                        Discount = 0.1m,
                        TotalAmount = itemTotal
                    });
                    s.TotalAmount += itemTotal;
                });

            var handler = new CreateSaleHandler(
                _saleRepositoryMock.Object,
                _userRepositoryMock.Object,
                _productRepositoryMock.Object,
                _saleServiceMock.Object,
                _mediatorMock.Object);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().NotBeEmpty();
            result.TotalAmount.Should().Be(90.0m); // 10 * 10 = 100 - 10% = 90
        }

        [Fact()]
        public async Task Handle_ShouldCreateSale_WithNoDiscount()
        {
            // Arrange
            var customerId = Guid.Parse("eea3ebf9-2ff6-4670-81ae-fcc22477ef96");
            var branchId = Guid.NewGuid();
            var productId = 2;

            var command = new CreateSaleCommand
            {
                CustomerId = customerId,
                BranchId = branchId,
                Items = new List<CreateSaleItemDto>
                    {
                        new() { ProductId = productId, Quantity = 3 }
                    }
            };

            var user = new User
            {
                Id = customerId,
                FirstName = "Test",
                LastName = "User",
                Email = "teste@ambev.com",
                Password = "Teste123",
                Role = Domain.Enums.UserRole.Customer,
                Status = Domain.Enums.UserStatus.Active
            };

            var product = new Product { Id = productId, Title = "Item A", Price = 10.0m };
            var sale = new Sale { Id = Guid.NewGuid(), Items = new List<SaleItem>(), TotalAmount = 0 };

            _userRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);
            _productRepositoryMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(product);

            _saleServiceMock.Setup(s => s.CreateSale(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(sale);

            _saleServiceMock.Setup(s => s.AddItem(It.IsAny<Sale>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<decimal>()))
                .Callback<Sale, string, string, int, decimal>((s, pid, pname, qty, price) =>
                {
                    var itemTotal = qty * price;
                    s.Items.Add(new SaleItem
                    {
                        Id = Guid.NewGuid(),
                        ProductId = pid,
                        ProductName = pname,
                        Quantity = qty,
                        UnitPrice = price,
                        Discount = 0.0m,
                        TotalAmount = itemTotal
                    });
                    s.TotalAmount += itemTotal;
                });

            var handler = new CreateSaleHandler(
                _saleRepositoryMock.Object,
                _userRepositoryMock.Object,
                _productRepositoryMock.Object,
                _saleServiceMock.Object,
                _mediatorMock.Object);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().NotBeEmpty();
            result.TotalAmount.Should().Be(30.0m); // 3 * 10 = 30
        }

        [Fact()]
        public async Task Handle_ShouldThrow_WhenCustomerNotFound()
        {
            // Arrange
            var customerId = Guid.Parse("eea3ebf9-2ff6-4670-81ae-fcc22477ef96");
            var branchId = Guid.NewGuid();
            var productId = 2;

            var command = new CreateSaleCommand
            {
                CustomerId = customerId,
                BranchId = branchId,
                Items = new List<CreateSaleItemDto>
        {
            new() { ProductId = productId, Quantity = 5 }
        }
            };

            _userRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((User?)null);

            var product = new Product { Id = productId, Title = "Item A", Price = 10.0m };
            _productRepositoryMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(product);

            var sale = new Sale { Id = Guid.NewGuid(), Items = new List<SaleItem>(), TotalAmount = 0 };
            _saleServiceMock.Setup(s => s.CreateSale(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(sale);

            var handler = new CreateSaleHandler(
                _saleRepositoryMock.Object,
                _userRepositoryMock.Object,
                _productRepositoryMock.Object,
                _saleServiceMock.Object,
                _mediatorMock.Object);

            // Act & Assert
            var act = async () => await handler.Handle(command, default);
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("*Customer not found*");
        }

        [Fact()]
        public async Task Handle_ShouldThrow_WhenProductNotFound()
        {
            // Arrange
            var customerId = Guid.Parse("eea3ebf9-2ff6-4670-81ae-fcc22477ef96");
            var branchId = Guid.NewGuid();
            var productId = 2;

            var command = new CreateSaleCommand
            {
                CustomerId = customerId,
                BranchId = branchId,
                Items = new List<CreateSaleItemDto>
        {
            new() { ProductId = productId, Quantity = 5 }
        }
            };

            var user = new User
            {
                Id = customerId,
                FirstName = "Test",
                LastName = "User",
                Email = "teste@ambev.com",
                Password = "Teste123",
                Role = Domain.Enums.UserRole.Customer,
                Status = Domain.Enums.UserStatus.Active
            };
            _userRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);

            _productRepositoryMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync((Product?)null);

            var sale = new Sale { Id = Guid.NewGuid(), Items = new List<SaleItem>(), TotalAmount = 0 };
            _saleServiceMock.Setup(s => s.CreateSale(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(sale);

            var handler = new CreateSaleHandler(
                _saleRepositoryMock.Object,
                _userRepositoryMock.Object,
                _productRepositoryMock.Object,
                _saleServiceMock.Object,
                _mediatorMock.Object);

            // Act & Assert
            var act = async () => await handler.Handle(command, default);
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("*Product with ID*");
        }

    }
}
