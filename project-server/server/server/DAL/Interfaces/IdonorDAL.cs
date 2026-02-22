using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.DAL.Interfaces
{
    public interface IdonorDAL
    {
        Task<List<DonorModel>> Get();
        Task<DonorModel?> GetById(int id);
        Task Post(DonorModel donorModel);
        Task Update(int id, DonorModel donorFromClient);
        Task Delete(int id);
        Task<DonorModel?> GetByGmail(string email);
        Task<DonorModel?> GetByName(string name);
        Task<List<DonorModel>> GetByGift(string gift);
    }
}