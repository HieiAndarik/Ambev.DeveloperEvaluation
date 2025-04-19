using Microsoft.AspNetCore.Mvc;
using Ambev.DeveloperEvaluation.Application.Carts;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces;

namespace Ambev.DeveloperEvaluation.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartsController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;

        public CartsController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCarts()
        {
            var carts = await _cartRepository.GetAllAsync();
            return Ok(carts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCartById(int id)
        {
            var cart = await _cartRepository.GetByIdAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            return Ok(cart);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCart([FromBody] Cart cart)
        {
            await _cartRepository.AddAsync(cart);
            return CreatedAtAction(nameof(GetCartById), new { id = cart.Id }, cart);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCart(int id, [FromBody] Cart cart)
        {
            var existingCart = await _cartRepository.GetByIdAsync(id);
            if (existingCart == null)
            {
                return NotFound();
            }

            existingCart.UserId = cart.UserId;
            existingCart.Items = cart.Items;

            await _cartRepository.UpdateAsync(existingCart);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(int id)
        {
            var success = await _cartRepository.DeleteByIdAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
