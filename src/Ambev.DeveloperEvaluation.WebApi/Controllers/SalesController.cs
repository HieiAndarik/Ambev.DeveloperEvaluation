using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;
using Ambev.DeveloperEvaluation.WebApi.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using Ambev.DeveloperEvaluation.Application.Sales.DTOs;
using Ambev.DeveloperEvaluation.Application.Common;

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
        public async Task<IActionResult> GetSales(
            [FromQuery] int page = 1,
            [FromQuery] int size = 10,
            [FromQuery] string order = "saleDate desc")
        {
            var query = new GetSalesQuery { Page = page, Size = size, Order = order };
            var result = await _mediator.Send(query);
            return Ok(new ApiResponseWithData<PagedResult<SaleDto>>(
                true, "Sales retrieved successfully", result));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSale(Guid id)
        {
            var query = new GetSaleQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(new ApiResponseWithData<SaleDetailDto>(
                true, "Sale retrieved successfully", result));
        }

        [HttpPost]
        public async Task<IActionResult> CreateSale([FromBody] CreateSaleCommand command)
        {
            var result = await _mediator.Send(command);
            return Created($"/api/sales/{result.Id}",
                new ApiResponseWithData<CreateSaleResult>(
                    true, "Sale created successfully", result));
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> CancelSale(Guid id)
        {
            var command = new CancelSaleCommand { Id = id };
            await _mediator.Send(command);
            return Ok(new ApiResponse(true, "Sale cancelled successfully"));
        }

        [HttpPost("{saleId}/items/{itemId}/cancel")]
        public async Task<IActionResult> CancelSaleItem(string saleId, string itemId)
        {
            var command = new CancelSaleItemCommand(saleId, itemId);
            await _mediator.Send(command);
            return Ok(new ApiResponse(true, "Sale item cancelled successfully"));
        }
    }
}