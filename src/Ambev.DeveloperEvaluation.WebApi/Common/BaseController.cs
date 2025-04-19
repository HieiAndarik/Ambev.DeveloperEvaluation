using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ambev.DeveloperEvaluation.WebApi.Common;

[Route("api/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
    protected int GetCurrentUserId() =>
            int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new NullReferenceException());

    protected string GetCurrentUserEmail() =>
        User.FindFirst(ClaimTypes.Email)?.Value ?? throw new NullReferenceException();

    protected IActionResult Ok<T>(T data) =>
            base.Ok(new ApiResponseWithData<T>(true, string.Empty, data));

    protected IActionResult Created<T>(string routeName, object routeValues, T data) =>
        base.CreatedAtRoute(routeName, routeValues, new ApiResponseWithData<T>(true, string.Empty, data));

    protected IActionResult BadRequest(string message) =>
        base.BadRequest(new ApiResponse(success: false, message: message));

    protected IActionResult NotFound(string message = "Resource not found") =>
        base.NotFound(new ApiResponse(success: false, message: message));

    protected IActionResult OkPaginated<T>(PaginatedList<T> pagedList) =>
        Ok(new PaginatedResponse<T>(
            success: true,
            message: string.Empty,
            data: pagedList,
            currentPage: pagedList.CurrentPage,
            totalPages: pagedList.TotalPages,
            totalCount: pagedList.TotalCount
        ));
}
