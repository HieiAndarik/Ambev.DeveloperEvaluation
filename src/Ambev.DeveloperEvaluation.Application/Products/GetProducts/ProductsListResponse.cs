namespace Ambev.DeveloperEvaluation.Application.Products.GetProducts
{
    public class ProductsListResponse<T>
    {
        public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
