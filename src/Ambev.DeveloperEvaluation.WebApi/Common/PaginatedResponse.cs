namespace Ambev.DeveloperEvaluation.WebApi.Common;

public class PaginatedResponse<T> : ApiResponseWithData<IEnumerable<T>>
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }

    public PaginatedResponse(bool success, string message, IEnumerable<T> data, int currentPage, int totalPages, int totalCount)
        : base(success, message, data)
    {
        CurrentPage = currentPage;
        TotalPages = totalPages;
        TotalCount = totalCount;
    }
}
