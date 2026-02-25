using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using server.BLL.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.DAL;
using WebApplication1.DTOs;
using WebApplication1.Models;

[ApiController]
[Route("api/[controller]")]
public class CustomerDetailsController : ControllerBase
{
    private readonly ICustomerDatailsBLL _customerBll;
    private readonly ILogger<CustomerDetailsController> _logger;
    private readonly LotteryContext _context; 
    public CustomerDetailsController(ICustomerDatailsBLL bll, ILogger<CustomerDetailsController> logger, LotteryContext context)
    {
        _customerBll = bll;
        _logger = logger;
        _context = context;
    }

    [Authorize(Roles = "manager")]
    [HttpGet("all")]
    public async Task<List<CustomerDetailsDto>> Get()
    {
        _logger.LogInformation("API: Fetching all customer details.");
        return await this._customerBll.Get();
    }

    [Authorize(Roles = "manager")]
    [HttpGet("by-gift/{giftId}")]
    public async Task<List<CustomerDetailsDto>> GetByGiftId(int giftId)
    {
        _logger.LogInformation("API: GetByGift called for GiftId: {GiftId}", giftId);
        return await this._customerBll.GetByGiftId(giftId);
    }

    [Authorize(Roles = "user")]
    [HttpPost]
    [Authorize(Roles = "user")]
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CustomerDetailsDto model)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return Unauthorized();

        model.CustomerId = int.Parse(userIdClaim.Value);

        await _customerBll.Add(model);
        return Ok();
    }

    [Authorize(Roles = "manager")]
    [HttpGet("reportRevenue")]
    public async Task<double> reportRevenue()
    {
        _logger.LogInformation("API: Revenue report requested.");
        double revenue = await _customerBll.reportRevenue();

        _logger.LogInformation("API: Revenue report calculated: {Revenue}", revenue);
        return revenue;
    }

    [Authorize(Roles = "user")]
    [HttpGet("ConfirmPurchase")]
    public async Task<IActionResult> ConfirmPurchase([FromQuery] int id)
    {
        _logger.LogInformation("API: Confirming purchase for ID: {Id}", id);
        await _customerBll.ConfirmPurchase(id);
        return Ok();
    }

    [Authorize(Roles = "user")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("API: Deleting donor ID: {Id}", id);
        await _customerBll.Delete(id);
        return Ok();
    }
    [Authorize(Roles = "user")]
    [HttpGet("GetMyPurchases")]
    public async Task<IActionResult> GetMyPurchases()
    {
        var userName = User.Identity?.Name;
        if (string.IsNullOrEmpty(userName)) return Unauthorized();

        var customer = await _context.Customers
                                     .FirstOrDefaultAsync(c => c.UserName.Trim() == userName.Trim());

        _logger.LogInformation("Search for user: '{UserName}', Found ID: {Id}", userName, customer?.Id);

        if (customer == null) return NotFound("Customer not found in DB");

        var results = await _customerBll.GetByCustomerId(customer.Id);
        return Ok(results);
    }

    [Authorize(Roles = "user")]
    [HttpGet("GetTotalAmount")]
    public async Task<ActionResult<double>> GetTotalAmount()
    {
        var userName = User.Identity?.Name;
        if (string.IsNullOrEmpty(userName)) return Unauthorized();

        var customer = await _context.Customers
                                     .FirstOrDefaultAsync(c => c.UserName.Trim() == userName.Trim());

        if (customer == null) return Ok(0.0); 
        double total = await _customerBll.GetTotalAmount(customer.Id);
        return Ok(total);
    }
}