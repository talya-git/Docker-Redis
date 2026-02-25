using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using server.BLL.Interfaces;
using server.DAL.Interfaces;
using WebApplication1.BLL;
using WebApplication1.BLL.Interfaces;
using WebApplication1.DAL;
using WebApplication1.DAL.Interfaces;
using WebApplication1.DTOs;
using WebApplication1.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace server.BLL
{
    public class CustomerDatailsBLL : ICustomerDatailsBLL
    {
        private readonly ICustomerDatailsDAL CustomerDatailsDAL;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomerDatailsBLL> _logger;

        public CustomerDatailsBLL(ICustomerDatailsDAL CustomerDatailsDAL, IMapper mapper, ILogger<CustomerDatailsBLL> logger)
        {
            this.CustomerDatailsDAL = CustomerDatailsDAL;
            this._mapper = mapper;
            this._logger = logger;
        }

        public async Task<List<CustomerDetailsDto>> GetByGiftId(int giftId)
        {
            _logger.LogInformation("Fetching details for gift ID: {GiftId}", giftId);
            return await this.CustomerDatailsDAL.GetByGiftId(giftId);
        }

        public async Task<CustomerDatails> GetByQuantity([FromQuery] int minQuantity)
        {
            _logger.LogInformation("Fetching details by quantity: {MinQuantity}", minQuantity);
            return await this.CustomerDatailsDAL.GetByQuantity(minQuantity);
        }

        public async Task Add(CustomerDetailsDto model)
        {
            _logger.LogInformation("Mapping and adding new customer detail for CustomerId: {CustomerId}", model.CustomerId);

            var donorModel = _mapper.Map<CustomerDatails>(model);

            if (donorModel.CustomerId == 0 && model.CustomerId != 0)
            {
                donorModel.CustomerId = model.CustomerId;
            }

            await CustomerDatailsDAL.Add(donorModel);
        }

        public async Task<double> reportRevenue()
        {
            _logger.LogInformation("Starting reportRevenue calculation");
            var allGifts = await CustomerDatailsDAL.reportRevenue();

            if (allGifts == null || !allGifts.Any())
            {
                _logger.LogWarning("reportRevenue: No gifts returned from DAL");
                return 0;
            }

            double total = 0;
            foreach (var gift in allGifts)
            {
                int listCount = gift.customerDatails?.Count ?? 0;
                int quantity = gift.customerDatails?.Sum(c => c.Quntity) ?? 0;

                _logger.LogDebug("Gift: {GiftName}, ListCount: {ListCount}, TotalQty: {Qty}, Price: {Price}",
                    gift.Name, listCount, quantity, gift.PriceCard);

                total += (quantity * gift.PriceCard);
            }

            _logger.LogInformation("Final reportRevenue: {Total}", total);
            return total;
        }

        public async Task ConfirmPurchase(int customerDetailsId)
        {
            _logger.LogInformation("Confirming purchase for ID: {Id}", customerDetailsId);
            await CustomerDatailsDAL.ConfirmPurchase(customerDetailsId);
        }

        public async Task<List<CustomerDetailsDto>> Get()
        {
            _logger.LogInformation("Fetching all customer details from DAL");
            var donorFromDb = await CustomerDatailsDAL.Get();

            _logger.LogInformation("Mapping {Count} records to DTOs", donorFromDb?.Count ?? 0);
            return _mapper.Map<List<CustomerDetailsDto>>(donorFromDb);
        }

        public async Task Delete(int id)
        {
            _logger.LogInformation("Attempting to delete donor with ID: {Id}", id);
            await CustomerDatailsDAL.Delete(id);
            _logger.LogInformation("Delete operation completed for donor ID: {Id}", id);
        }
        public async Task<List<CustomerDetailsDto>> GetByCustomerId(int userId)
        {
            var purchases = await CustomerDatailsDAL.GetByCustomerId(userId);
            return _mapper.Map<List<CustomerDetailsDto>>(purchases);
        }
        public async Task<double> GetTotalAmount(int userId)
        {
            double total = await CustomerDatailsDAL.GetTotalAmountByCustomerId(userId);

            _logger.LogInformation($"Total amount calculated for user {userId}: {total}");
            return total;
        }
    }
}