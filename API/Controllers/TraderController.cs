using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Domain.Entities;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TraderController : ControllerBase
    {
        private readonly ITraderManagement _traderManagement;

        public TraderController(ITraderManagement traderManagement)
        {
            _traderManagement = traderManagement;
        }

        [HttpPost("add-trader")]
        public async Task<IActionResult> AddTrader([FromBody] Trader trader)
        {
            if (!ModelState.IsValid) // Check for validation errors
            {
                return BadRequest(ModelState); // Return 400 Bad Request with validation errors
            }
            try
            {
                await _traderManagement.AddTraderAsync(trader);
                return Ok("Trader added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error adding trader: {ex.Message}");
            }
        }
        [HttpDelete("delete-trader/{traderId:guid}")]
        public async Task<IActionResult> DeleteTrader(Guid traderId)
        {
            try
            {
                await _traderManagement.DeleteTraderAsync(traderId);
                return NoContent(); 
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting trader: {ex.Message}");
            }
        }
        [HttpGet("get-all-traders")]
        public async Task<IActionResult> GetAllTraders()
        {
            try
            {
                var traders = await _traderManagement.GetAllTradersAsync();

                if (traders == null || !traders.Any())
                    return NotFound("No traders found.");

                return Ok(traders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving traders: {ex.Message}");
            }
        }
        [HttpGet("get-trader/{traderId}")]
        public async Task<IActionResult> GetTraderById(Guid traderId)
        {
            try
            {
                var trader = await _traderManagement.GetTraderByIdAsync(traderId);

                if (trader == null)
                    return NotFound($"Trader with ID {traderId} not found.");

                return Ok(trader);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving trader: {ex.Message}");
            }
        }

    }
}
