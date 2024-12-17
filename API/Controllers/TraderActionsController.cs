using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Handles actions related to a trader, specifically placing orders.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TraderActionsController : ControllerBase
    {
        private readonly ITraderActions _traderActions;

        public TraderActionsController(ITraderActions traderActions)
        {
            _traderActions = traderActions;
        }

        [HttpPost("{traderId}/place-order")]
        public async Task<IActionResult> PlaceOrder(Guid traderId, [FromBody] OrderRequest orderRequest)
        {
            if (orderRequest == null || traderId == Guid.Empty)
                return BadRequest("Invalid request data.");
            try
            {
                await _traderActions.PlaceOrderAsync(traderId, orderRequest.StockSymbol, orderRequest.Quantity, orderRequest.Price, orderRequest.OrderType);
                return Ok("Order placed successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }

    public class OrderRequest
    {
        public string StockSymbol { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string OrderType { get; set; } = string.Empty; // "buy" or "sell"
    }
}
