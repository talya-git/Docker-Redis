using WebApplication1.DTOs;
using WebApplication1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication1.BLL.Interfaces
{
    public interface IcustomerBLL
    {
        Task<List<CustomerDto>> Get();
        Task<CustomerDto> GetById(int id);
    }
}