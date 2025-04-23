using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;

public sealed class DeleteCartValidator : AbstractValidator<DeleteCartCommand>
{
    public DeleteCartValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Cart ID must be greater than zero.");
    }
}
