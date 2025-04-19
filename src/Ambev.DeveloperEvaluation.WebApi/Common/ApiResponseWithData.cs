namespace Ambev.DeveloperEvaluation.WebApi.Common
{
    public class ApiResponseWithData<T> : ApiResponse
    {
        public T Data { get; set; }

        public ApiResponseWithData(bool success, string message, T data)
            : base(success, message)
        {
            Data = data;
        }
    }
}