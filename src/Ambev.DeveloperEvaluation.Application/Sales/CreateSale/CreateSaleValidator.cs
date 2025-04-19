using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleValidator : AbstractValidator<CreateSaleCommand>
    {
        public CreateSaleValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty().WithMessage("Customer is required");
            RuleFor(x => x.BranchId).NotEmpty().WithMessage("Branch is required");
            RuleFor(x => x.Items).NotEmpty().WithMessage("At least one item is required");

            RuleForEach(x => x.Items).ChildRules(item => {
                item.RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product is required");
                item.RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than zero")
                                         .LessThanOrEqualTo(20).WithMessage("Quantity cannot exceed 20 items");
            });
        }
    }
}