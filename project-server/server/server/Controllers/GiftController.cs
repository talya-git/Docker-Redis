using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

    public GiftController(IgiftBLL gift, ILogger<GiftController> logger)
    {
        this.giftBLL = gift;
        this._logger = logger;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<List<GiftDto>>> Get()
    {
        _logger.LogInformation("API Request: Fetching all gifts.");
        var result = await this.giftBLL.Get();
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<GiftDto>> GetById(int id)
    {
        _logger.LogInformation("API: Requesting gift by ID: {Id}", id);
        var gift = await giftBLL.GetById(id);

        if (gift == null)
        {
            _logger.LogWarning("API: Gift with ID {Id} not found.", id);
            return NotFound();
        }

        return Ok(gift);
    }

    [Authorize(Roles = "manager")]
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] GiftDto gift)
    {
        _logger.LogInformation("API Request: Creating a new gift: {GiftName}", gift?.Name);
        await giftBLL.Post(gift);
        return Ok();
    }

    [Authorize(Roles = "manager")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] GiftDto gift)
    {
        _logger.LogInformation("API Request: Updating gift ID: {Id}", id);
        await giftBLL.Update(id, gift);
        return Ok();
    }

    [Authorize(Roles = "manager")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("API Request: Deleting gift ID: {Id}", id);
        await giftBLL.Delete(id);
        return Ok();
    }

    [AllowAnonymous]
    [HttpGet("by-name")]
    public async Task<ActionResult<GiftDto>> GetByName([FromQuery] string name)
    {
        _logger.LogInformation("API Request: Search gift by name: {Name}", name);
        var result = await giftBLL.GetByName(name);
        return Ok(result);
    }

    [Authorize(Roles = "manager")]
    [HttpGet("by-parches-count")]
    public async Task<ActionResult<List<GiftDto>>> GetByPurchasesCount([FromQuery] int num)
    {
        _logger.LogInformation("API Request: Search gift by parches: {num}", num);
        var result = await giftBLL.GetByPurchasesCount(num);
        return Ok(result);
    }

    [Authorize(Roles = "manager")]
    [HttpGet("by-donor")]
    public async Task<ActionResult<List<GiftDto>>> GetByDonor([FromQuery] string name)
    {
        _logger.LogInformation("API Request: Search gift by name: {Name}", name);
        var result = await giftBLL.GetByDonor(name);
        return Ok(result);
    }

    [Authorize(Roles = "manager")]
    [HttpPost("winner/{giftId}")]
    public async Task<ActionResult<WinnerDTO>> Winner(int giftId)
    {
        _logger.LogInformation("API Request: Starting lottery for gift ID: {Id}", giftId);
        var winnerDto = await giftBLL.Winner(giftId);
        return Ok(winnerDto);
    }

    [Authorize(Roles = "manager")]
    [HttpGet("reportWinners")]
    public async Task<ActionResult<List<GiftDto>>> reportWinners()
    {
        _logger.LogInformation("API Request: Generating winners report.");
        var result = await giftBLL.reportWinners();
        return Ok(result);
    }

    [Authorize(Roles = "manager")]
    [HttpGet("giftExpensive")]
    public async Task<ActionResult<List<GiftDto>>> giftExpensive()
    {
        _logger.LogInformation("API Request: Fetching most expensive gift.");
        var result = await giftBLL.giftExpensive();
        return Ok(result);
    }

    [Authorize(Roles = "manager")]
    [HttpGet("reportAchnasot")]
    public async Task<ActionResult<int>> reportAchnasot()
    {
        _logger.LogInformation("API Request: Fetching revenue report.");
        var result = await this.giftBLL.reportAchnasot();
        return Ok(result);
    }
}