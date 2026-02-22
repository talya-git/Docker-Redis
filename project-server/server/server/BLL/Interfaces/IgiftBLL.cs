using Microsoft.AspNetCore.Mvc;
using server.DTO;
using WebApplication1.DTOs;
using WebApplication1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication1.BLL.Interfaces
{
    public interface IgiftBLL
    {
        Task<List<GiftDto>> Get();
        Task<GiftDto> GetById(int id);

        Task Post(GiftDto giftModel);
        Task Delete(int id);

        Task Update(int id, GiftDto gift);
        Task<GiftDto> GetByName(string name);
        Task<List<GiftDto>> GetByDonor(string name);
        Task<List<CustomerDetailsDto>> GetByCategory(string category);

        Task<List<CustomerDetailsDto>> GetByParches(int id);
        Task<List<GiftDto>> GetByPurchasesCount(int num);
        Task<List<GiftDto>> giftExpensive();
        Task<GiftDto> byMostParches();

        Task<WinnerDTO> Winner(int giftId);
        Task<List<GiftDto>> reportWinners();
        Task<int> reportAchnasot();
    }
}