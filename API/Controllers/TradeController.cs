using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Domain.Entities;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TradeController : ControllerBase
{
    private readonly ITraderService _traderService;

    public TradeController(ITraderService traderService)
    {
        _traderService = traderService;
    }

    [HttpPost("place-order")]
    public async Task<IActionResult> PlaceOrder([FromBody] StockOrder request)
    {
        if (request == null)
        {
            return BadRequest("Invalid order request.");
        }

        if (request.TraderId == Guid.Empty)
        {
            return BadRequest("Trader ID is required.");
        }

        if (request.Quantity <= 0)
        {
            return BadRequest("Quantity must be greater than zero.");
        }

        if (request.Price <= 0)
        {
            return BadRequest("Price must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(request.OrderType) || 
            (request.OrderType.ToLower() != "buy" && request.OrderType.ToLower() != "sell"))
        {
            return BadRequest("OrderType must be 'buy' or 'sell'.");
        }

        try
        {
            await _traderService.PlaceOrderAsync(
                request.TraderId,
                request.StockSymbol,
                request.Quantity,
                request.Price,
                request.OrderType
            );

            return Ok("Order placed successfully.");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }

    [HttpGet("traders")]
    public async Task<IActionResult> GetAllTraders()
    {
        try
        {
            var traders = await _traderService.GetAllTradersAsync();
            return Ok(traders);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }

    [HttpGet("trader/{traderId}/orders")]
    public async Task<IActionResult> GetOrdersForTrader(Guid traderId)
    {
        if (traderId == Guid.Empty)
        {
            return BadRequest("Trader ID is required.");
        }

        try
        {
            var trader = await _traderService.GetTraderByIdAsync(traderId);
            if (trader == null)
            {
                return NotFound("Trader not found.");
            }

            return Ok(trader.Orders);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
}
