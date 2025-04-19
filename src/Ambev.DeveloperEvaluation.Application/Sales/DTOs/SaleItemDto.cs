namespace Ambev.DeveloperEvaluation.Application.Sales.DTOs
{
    public class SaleItemDto
    {
        public SaleItemDto()
        {
            ProductName = string.Empty;
        }
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsCancelled { get; set; }
    }
}