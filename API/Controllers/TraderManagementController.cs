using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Domain.Entities;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TraderManagementController : ControllerBase
    {
        private readonly ITraderManagement _traderManagement;

        public TraderManagementController(ITraderManagement traderManagement)
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
    }
}
