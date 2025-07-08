using MarketPriceAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MarketPriceAPI.Controllers
{
    [Route("api/historicalPrices")]
    [ApiController]
    public class HistoricalPriceController : ControllerBase
    {
        private readonly IHistoricalPriceService _historicalPriceService;
        public HistoricalPriceController(IHistoricalPriceService historicalPriceService)
        {
            _historicalPriceService = historicalPriceService;
        }

        [HttpGet("latest-bars")]
        public async Task<IActionResult> GetLatestBars(
            [FromQuery] Guid instrumentId,
            [FromQuery] string? provider = null,
            [FromQuery] int? interval = null,
            [FromQuery] string? periodicity = null,
            [FromQuery] int barsCount = 10)
        {
            try
            {
                var bars = await _historicalPriceService.GetLatestBarsAsync(instrumentId, provider, interval, periodicity, barsCount);
                if (bars == null || !bars.Any())
                {
                    return NotFound("No bars found for the specified criteria.");
                }
                return Ok(bars);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("date-range")]
        public async Task<IActionResult> GetBarsByDateRange(
            [FromQuery] Guid instrumentId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] string? provider = null,
            [FromQuery] int? interval = null,
            [FromQuery] string? periodicity = null)
        {
            try
            {
                var bars = await _historicalPriceService.GetBarsByDateRangeAsync(instrumentId, startDate, endDate, provider, interval, periodicity);
                if (bars == null || !bars.Any())
                {
                    return NotFound("No bars found for the specified date range.");
                }
                return Ok(bars);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("time-back")]
        public async Task<IActionResult> GetBarsByTimeBack(
            [FromQuery] Guid instrumentId,
            [FromQuery] TimeSpan timeBack,
            [FromQuery] string? provider = null,
            [FromQuery] int? interval = null,
            [FromQuery] string? periodicity = null)
        {
            try
            {
                var bars = await _historicalPriceService.GetBarsByTimeBackAsync(instrumentId, timeBack, provider, interval, periodicity);
                if (bars == null || !bars.Any())
                {
                    return NotFound("No bars found for the specified time back.");
                }
                return Ok(bars);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
    }
}
