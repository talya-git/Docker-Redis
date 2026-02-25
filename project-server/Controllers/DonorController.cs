using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebApplication1.BLL.Interfaces;
using WebApplication1.DTOs;
using WebApplication1.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

[Authorize(Roles = "manager")]
[ApiController]
[Route("api/[controller]")]
public class DonorController : ControllerBase
{
    private readonly IdonorBLL donorBLL;
    private readonly ILogger<DonorController> _logger;

    public DonorController(IdonorBLL bll, ILogger<DonorController> logger)
    {
        this.donorBLL = bll;
        this._logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<DonorDto>>> Get()
    {
        _logger.LogInformation("API: Requesting all donors.");
        var result = await this.donorBLL.Get();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DonorDto>> GetById(int id)
    {
        _logger.LogInformation("API: Requesting donor by ID: {Id}", id);
        var donor = await donorBLL.GetById(id);

        if (donor == null)
        {
            _logger.LogWarning("API: Donor with ID {Id} not found.", id);
            return NotFound();
        }

        return Ok(donor);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] DonorDto donor)
    {
        if (donor == null) return BadRequest();

        if (donor.Id == 0)
        {
            _logger.LogInformation("API: Adding a new donor.");
            await this.donorBLL.Post(donor);
        }
        else
        {
            _logger.LogInformation("API: Updating existing donor with ID: {Id}", donor.Id);
            var donorDto = new DonorDto
            {
                Id = donor.Id,
                FirstName = donor.FirstName,
                LastName = donor.LastName,
                Phone = donor.Phone,
                Email = donor.Email,
                Address = donor.Address,
            };
            await this.donorBLL.Update(donor.Id, donorDto);
        }
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] DonorDto donor)
    {
        _logger.LogInformation("API: Updating donor ID: {Id}", id);
        await donorBLL.Update(id, donor);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("API: Deleting donor ID: {Id}", id);
        await donorBLL.Delete(id);
        return Ok();
    }

    [HttpGet("by-email")]
    public async Task<ActionResult<DonorDto>> GetByEmail([FromQuery] string email)
    {
        _logger.LogInformation("API: Searching donor by email: {Email}", email);
        var donor = await donorBLL.GetByEmail(email);

        if (donor == null)
        {
            _logger.LogWarning("API: Donor with email {Email} not found.", email);
            return NotFound();
        }

        return Ok(donor);
    }

    [HttpGet("by-name")]
    public async Task<ActionResult<DonorDto>> GetByName([FromQuery] string name)
    {
        _logger.LogInformation("API: Searching donor by name: {Name}", name);
        var donor = await donorBLL.GetByName(name);

        if (donor == null)
        {
            _logger.LogWarning("API: Donor with name {Name} not found.", name);
            return NotFound();
        }

        return Ok(donor);
    }

    [HttpGet("by-gift")]
    public async Task<ActionResult<List<DonorDto>>> GetByGift([FromQuery] string name)
    {
        _logger.LogInformation("API: Searching donor for gift ID: {Id}", name);
        var donor = await donorBLL.GetByGift(name);

        if (donor == null)
        {
            _logger.LogWarning("API: No donor found for gift ID: {Id}", name);
            return NotFound();
        }

        return Ok(donor);
    }
}