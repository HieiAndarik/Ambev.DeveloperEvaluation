using Ambev.DeveloperEvaluation.Domain.Entities;
using System.Text.Json.Serialization;

public class CartItem
{
    public int Id { get; set; } 
    public int ProductId { get; set; }
    public int Quantity { get; set; }

    public int CartId { get; set; }
    [JsonIgnore]
    public Cart? Cart { get; set; }
}
