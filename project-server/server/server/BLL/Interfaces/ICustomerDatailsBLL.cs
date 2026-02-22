using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;
using WebApplication1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace server.BLL.Interfaces
{
    public interface ICustomerDatailsBLL
    {
        Task<List<CustomerDetailsDto>> Get();

        Task<List<CustomerDetailsDto>> GetByGiftId(int giftId);

        Task<CustomerDatails> GetByQuantity([FromQuery] int minQuantity);

        Task Add(CustomerDetailsDto model);

        Task<double> reportRevenue();

        Task ConfirmPurchase(int customerDetailsId);

        Task Delete(int id);
        Task<List<CustomerDetailsDto>> GetByCustomerId(int userId);
        Task<double> GetTotalAmount(int userId);
    }
}