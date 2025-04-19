using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.WebApi.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SalesController : BaseController
    {
        private readonly IMediator _mediator;

        public SalesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponseWithData<PaginatedList<SaleDto>>>> GetSales(
            [FromQuery] int page = 1,
            [FromQuery] int size = 10,
            [FromQuery] string order = "saleDate desc")
        {
            var query = new GetSalesQuery { Page = page, Size = size, Order = order };
            var result = await _mediator.Send(query);
            return Ok(new ApiResponseWithData<PaginatedList<SaleDto>>(
                true, "Sales retrieved successfully", result));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponseWithData<SaleDetailDto>>> GetSale(int id)
        {
            var query = new GetSaleQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(new ApiResponseWithData<SaleDetailDto>(
                true, "Sale retrieved successfully", result));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponseWithData<CreateSaleResult>>> CreateSale(
            [FromBody] CreateSaleCommand command)
        {
            var result = await _mediator.Send(command);
            return Created($"/api/sales/{result.Id}",
                new ApiResponseWithData<CreateSaleResult>(
                    true, "Sale created successfully", result));
        }

        [HttpPost("{id}/cancel")]
        public async Task<ActionResult<ApiResponse>> CancelSale(int id)
        {
            var command = new CancelSaleCommand { Id = id };
            await _mediator.Send(command);
            return Ok(new ApiResponse(true, "Sale cancelled successfully"));
        }

        [HttpPost("{saleId}/items/{itemId}/cancel")]
        public async Task<ActionResult<ApiResponse>> CancelSaleItem(int saleId, int itemId)
        {
            var command = new CancelSaleItemCommand { SaleId = saleId, ItemId = itemId };
            await _mediator.Send(command);
            return Ok(new ApiResponse(true, "Sale item cancelled successfully"));
        }
    }
}