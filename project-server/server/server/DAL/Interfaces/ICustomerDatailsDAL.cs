using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.DTOs;
using WebApplication1.Models;

namespace server.DAL.Interfaces
{
    public interface ICustomerDatailsDAL
    {
        Task<List<CustomerDetailsDto>> Get();
        Task Add(CustomerDatails model);
        Task ConfirmPurchase(int customerDetailsId);

        Task<CustomerDatails> GetByQuantity(int minQuantity);

        Task<List<CustomerDetailsDto>> GetByGiftId(int giftId);
        Task Delete(int id);
        Task<List<CustomerDetailsDto>> GetByCustomerId(int userId);
        Task<double> GetTotalAmountByCustomerId(int userId);
        Task<List<GiftModel>> reportRevenue();
    }
}