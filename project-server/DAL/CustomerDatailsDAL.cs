using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using server.DAL.Interfaces;
using WebApplication1.DAL;
using WebApplication1.DTOs;
using WebApplication1.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using AutoMapper; 

namespace server.DAL
{
    public class CustomerDatailsDAL : ICustomerDatailsDAL
    {
        private readonly LotteryContext LotteryContext;
        private readonly ILogger<CustomerDatailsDAL> _logger;
        private readonly IMapper _mapper; 

        public CustomerDatailsDAL(LotteryContext _lotteryContext, ILogger<CustomerDatailsDAL> logger, IMapper mapper)
        {
            this.LotteryContext = _lotteryContext;
            this._logger = logger;
            this._mapper = mapper;
        }

        public async Task<List<CustomerDetailsDto>> Get()
        {
            var details = await this.LotteryContext.CustomerDetails
                .Include(cd => cd.customer)
                .Include(cd => cd.gift)
                .ToListAsync();

            return _mapper.Map<List<CustomerDetailsDto>>(details);
        }

        public async Task Add(CustomerDatails model)
        {
           

            var customerExists = await this.LotteryContext.Customers.AnyAsync(c => c.Id == model.CustomerId);

            if (!customerExists)
            {
                _logger.LogWarning("Add failed: Customer with ID {CustomerId} not found.", model.CustomerId);
                throw new Exception($"Customer with ID {model.CustomerId} does not exist in the database.");
            }

            var existingItem = await this.LotteryContext.CustomerDetails
                .FirstOrDefaultAsync(d => d.CustomerId == model.CustomerId && d.GiftId == model.GiftId);

            if (existingItem != null)
            {
                existingItem.Quntity += model.Quntity;
                _logger.LogInformation("Updated quantity for Gift ID {GiftId} for Customer ID {CustomerId}", model.GiftId, model.CustomerId);
            }
            else
            {
                model.customer = null;
                model.gift = null;
                model.Status = "Draft";

                await this.LotteryContext.CustomerDetails.AddAsync(model);
                _logger.LogInformation("Successfully added new item to cart for Customer ID {CustomerId}", model.CustomerId);
            }

            await this.LotteryContext.SaveChangesAsync();
        }
        public async Task ConfirmPurchase(int customerDetailsId)
        {
            var customerDetails = await this.LotteryContext.CustomerDetails.FindAsync(customerDetailsId);
            if (customerDetails != null)
            {
                customerDetails.Status = "Confirmed";
                await this.LotteryContext.SaveChangesAsync();
            }
        }

        public async Task<CustomerDatails> GetByQuantity([FromQuery] int minQuantity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CustomerDetailsDto>> GetByGiftId(int giftId)
        {
            var details = await this.LotteryContext.CustomerDetails
                .Include(cd => cd.customer)
                .Include(cd => cd.gift)
                .Where(cd => cd.GiftId == giftId)
                .ToListAsync();

            return _mapper.Map<List<CustomerDetailsDto>>(details);
        }

        public async Task<List<GiftModel>> reportRevenue()
        {
            return await this.LotteryContext.Gifts
                .Include(g => g.customerDatails)
                .ToListAsync();
        }

        public async Task Delete(int id)
        {
            var c = await LotteryContext.CustomerDetails.FindAsync(id);

            if (c == null)
                return;

            if (c.Status == "Draft")
            {
                LotteryContext.CustomerDetails.Remove(c);
                await LotteryContext.SaveChangesAsync();
            }
            else
            {
                _logger.LogWarning("Delete rejected: Cannot delete purchase ID {Id} because its status is '{Status}'. Only 'Draft' purchases can be deleted.", id, c.Status);
            }
        }

        public async Task<List<CustomerDetailsDto>> GetByCustomerId(int userId)
        {
            var details = await LotteryContext.CustomerDetails
                .Include(x => x.customer) 
                .Include(x => x.gift)
                .Where(x => x.CustomerId == userId)
                .ToListAsync();

            return _mapper.Map<List<CustomerDetailsDto>>(details);
        }

        public async Task<double> GetTotalAmountByCustomerId(int userId)
        {
            return await LotteryContext.CustomerDetails
                .Where(x => x.CustomerId == userId)
                .SumAsync(x => x.gift.PriceCard * x.Quntity);
        }
    }
}