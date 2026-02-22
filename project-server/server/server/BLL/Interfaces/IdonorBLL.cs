using WebApplication1.DTOs;
using WebApplication1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication1.BLL.Interfaces
{
    public interface IdonorBLL
    {
        Task<List<DonorDto>> Get();
        Task<DonorDto> GetById(int id);

        Task Delete(int id);

        Task Post(DonorDto donor);

        Task Update(int id, DonorDto donor);

        Task<DonorDto> GetByEmail(string email);
        Task<DonorDto> GetByName(string name);
        Task<List<DonorDto>> GetByGift(string name);
    }
}