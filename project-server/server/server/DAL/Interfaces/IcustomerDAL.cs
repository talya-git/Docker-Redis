using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.DAL.Interfaces
{
    public interface IcustomerDAL
    {
        Task<List<CustomerModel>> Get();

        Task<CustomerModel?> GetById(int id);
    }
}