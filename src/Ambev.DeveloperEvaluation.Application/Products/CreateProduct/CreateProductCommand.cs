using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

public sealed class CreateProductCommand : IRequest<int>
{
    public CreateProductCommand(string title, decimal price, string category, string image, string description, double rate, int count)
    {
        Title = title;
        Price = price;
        Category = category;
        Image = image;
        Description = description;
        Rate = rate;
        Count = count;
    }

    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public string Category { get; init; } = string.Empty;
    public string Image { get; init; } = string.Empty;
    public double Rate { get; init; }
    public int Count { get; init; }
}
