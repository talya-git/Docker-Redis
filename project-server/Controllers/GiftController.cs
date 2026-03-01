using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using server.DAL;
using server.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.BLL;
using WebApplication1.BLL.Interfaces;
using WebApplication1.DTOs;
using WebApplication1.Models;

[ApiController]
[Route("api/[controller]")]
public class GiftController : ControllerBase
{
    private readonly IgiftBLL giftBLL;
    private readonly ILogger<GiftController> _logger;
    private readonly IDistributedCache _cache;

    public GiftController(IgiftBLL gift, ILogger<GiftController> logger, IDistributedCache cache)
    {
        this.giftBLL = gift;
        this._logger = logger;
        this._cache = cache;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<List<GiftDto>>> Get()
    {
        string cacheKey = "all_gifts_list";
        var cachedGifts = await _cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cachedGifts))
        {
            var gifts = JsonSerializer.Deserialize<List<GiftDto>>(cachedGifts);
            return Ok(gifts);
        }

        var result = await this.giftBLL.Get();

        var cacheOptions = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

        var serializedGifts = JsonSerializer.Serialize(result);
        await _cache.SetStringAsync(cacheKey, serializedGifts, cacheOptions);

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<GiftDto>> GetById(int id)
    {
        var gift = await giftBLL.GetById(id);

        if (gift == null)
        {
            return NotFound();
        }

        return Ok(gift);
    }

    [Authorize(Roles = "manager")]
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] GiftDto gift)
    {
        await giftBLL.Post(gift);
        await _cache.RemoveAsync("all_gifts_list");
        return Ok();
    }

    [Authorize(Roles = "manager")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] GiftDto gift)
    {
        await giftBLL.Update(id, gift);
        await _cache.RemoveAsync("all_gifts_list");
        return Ok();
    }

    [Authorize(Roles = "manager")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await giftBLL.Delete(id);
        await _cache.RemoveAsync("all_gifts_list");
        return Ok();
    }

    [AllowAnonymous]
    [HttpGet("by-name")]
    public async Task<ActionResult<GiftDto>> GetByName([FromQuery] string name)
    {
        var result = await giftBLL.GetByName(name);
        return Ok(result);
    }

    [Authorize(Roles = "manager")]
    [HttpGet("by-parches-count")]
    public async Task<ActionResult<List<GiftDto>>> GetByPurchasesCount([FromQuery] int num)
    {
        var result = await giftBLL.GetByPurchasesCount(num);
        return Ok(result);
    }

    [Authorize(Roles = "manager")]
    [HttpGet("by-donor")]
    public async Task<ActionResult<List<GiftDto>>> GetByDonor([FromQuery] string name)
    {
        var result = await giftBLL.GetByDonor(name);
        return Ok(result);
    }

    [Authorize(Roles = "manager")]
    [HttpPost("winner/{giftId}")]
    public async Task<ActionResult<WinnerDTO>> Winner(int giftId)
    {
        var winnerDto = await giftBLL.Winner(giftId);
        return Ok(winnerDto);
    }

    [Authorize(Roles = "manager")]
    [HttpGet("reportWinners")]
    public async Task<ActionResult<List<GiftDto>>> reportWinners()
    {
        var result = await giftBLL.reportWinners();
        return Ok(result);
    }

    [Authorize(Roles = "manager")]
    [HttpGet("giftExpensive")]
    public async Task<ActionResult<List<GiftDto>>> giftExpensive()
    {
        var result = await giftBLL.giftExpensive();
        return Ok(result);
    }

    [Authorize(Roles = "manager")]
    [HttpGet("reportAchnasot")]
    public async Task<ActionResult<int>> reportAchnasot()
    {
        var result = await this.giftBLL.reportAchnasot();
        return Ok(result);
    }
}