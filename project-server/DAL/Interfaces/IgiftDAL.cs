using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1.DAL.Interfaces
{
    public interface IgiftDAL
    {
        Task<List<GiftModel>> Get();
        Task<GiftModel?> GetById(int id);
        Task Post(GiftModel giftModel);
        Task Update(int id, GiftModel GiftFromClient);
        Task Delete(int id);

        Task<GiftModel?> GetByName(string name);
        Task<GiftModel?> GetByCategory(string name);
        Task<List<GiftModel>> GetByDonor(string name);

        Task<List<GiftModel>> GetByParches(int count);
        Task<List<GiftModel>> GetByPurchasesCount(int num);

        Task<List<GiftModel>> giftExpensive();
        Task<GiftModel?> byMostParches();

        Task<GiftModel?> Winner(int giftId);
        Task<List<GiftDto>> reportWinners();
        Task<int> reportAchnasot();
    }
}