using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.BLL.Interfaces;
using WebApplication1.DTOs;
using WebApplication1.Models;

[Authorize(Roles = "manager")]
[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly IcustomerBLL customerBLL;
    private readonly ILogger<CustomerController> _logger;

    public CustomerController(IcustomerBLL bll, ILogger<CustomerController> logger)
    {
        this.customerBLL = bll;
        this._logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<CustomerDto>>> Get()
    {
        _logger.LogInformation("API Request: Fetching all customers.");

        var result = await this.customerBLL.Get();

        if (result == null || result.Count == 0)
        {
            _logger.LogWarning("API Response: No customers found or list is empty.");
        }
        else
        {
            _logger.LogInformation("API Response: Returning {Count} customers.", result.Count);
        }

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerDto>> GetById(int id)
    {
        _logger.LogInformation("API: Requesting donor by ID: {Id}", id);
        var c = await customerBLL.GetById(id);

        if (c == null)
        {
            _logger.LogWarning("API: Donor with ID {Id} not found.", id);
            return NotFound();
        }

        return Ok(c);
    }
}