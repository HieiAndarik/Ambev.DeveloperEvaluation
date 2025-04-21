using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using FluentValidation;

public sealed class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductValidator()
    {
        RuleFor(p => p.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(100);

        RuleFor(p => p.Description)
            .NotEmpty().WithMessage("Description is required");

        RuleFor(p => p.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero");

        RuleFor(p => p.Category)
            .NotEmpty().WithMessage("Category is required");

        RuleFor(p => p.Image)
            .NotEmpty().WithMessage("Image is required")
            .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute)).WithMessage("Image must be a valid URL");

        RuleFor(p => p.Rate)
            .InclusiveBetween(0, 5).WithMessage("Rate must be between 0 and 5");

        RuleFor(p => p.Count)
            .GreaterThanOrEqualTo(0).WithMessage("Count must be a non-negative number");
    }
}
