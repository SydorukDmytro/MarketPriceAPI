using MarketPriceAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MarketPriceAPI.Controllers
{
    [Route("api/realtimeprices")]
    [ApiController]
    public class RealTimePriceController : ControllerBase
    {
        private readonly IRealTimePriceService _realTimePriceService;

        public RealTimePriceController(IRealTimePriceService realTimePriceService)
        {
            _realTimePriceService = realTimePriceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLatestPrice([FromQuery] Guid instrumentId)
        {
            if (instrumentId == Guid.Empty)
                return BadRequest("Invalid instrumentId");

            var priceSnapshot = await _realTimePriceService.GetLatestPriceAsync(instrumentId);

            if (priceSnapshot == null)
                return NotFound($"No real-time price found for instrument ID: {instrumentId}");

            return Ok(priceSnapshot);
        }
    }
}
