using MarketPriceAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MarketPriceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetController : ControllerBase
    {
        private readonly IAssetService assetService;
        public AssetController(IAssetService assetService)
        {
            this.assetService = assetService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAssets([FromQuery] string? provider = null, [FromQuery] string? kind = null)
        {
            try
            {
                var assets = await assetService.GetAllAssetsAsync(provider, kind);
                if (assets == null || !assets.Any())
                {
                    return NotFound("No assets found.");
                }
                return Ok(assets);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
    }
}
