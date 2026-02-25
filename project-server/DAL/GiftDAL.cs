using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.DAL.Interfaces;
using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1.DAL
{
    public class GiftDAL : IgiftDAL
    {
        private readonly LotteryContext lotteryContext;

        public GiftDAL(LotteryContext lotteryContext)
        {
            this.lotteryContext = lotteryContext;
        }

        public async Task<List<GiftModel>> Get()
        {
            return await lotteryContext.Gifts
                                 .Include(g => g.Winner) 
                                 .Include(g => g.Donor)
                                 .ToListAsync();
        }

        public async Task<GiftModel> GetById(int id)
        {
            return await lotteryContext.Gifts.FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task Post(GiftModel giftModel)
        {
            if (giftModel.WinnerId == 0) giftModel.WinnerId = null;

            giftModel.Donor = null;
            giftModel.Winner = null;

            await this.lotteryContext.Gifts.AddAsync(giftModel);
            await this.lotteryContext.SaveChangesAsync();
        }

        public async Task Update(int id, GiftModel GiftFromClient)
        {
            var giftFromDb = await lotteryContext.Gifts.FindAsync(id);
            if (giftFromDb == null) return;

            giftFromDb.Name = GiftFromClient.Name;
            giftFromDb.PriceCard = GiftFromClient.PriceCard;
            giftFromDb.DonorId = GiftFromClient.DonorId;
            giftFromDb.GiftImage = GiftFromClient.GiftImage;

            giftFromDb.Donor = null;

            if (GiftFromClient.WinnerId != null && GiftFromClient.WinnerId != 0)
                giftFromDb.WinnerId = GiftFromClient.WinnerId;

            await lotteryContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var gift = await lotteryContext.Gifts.FindAsync(id);
            if (gift == null) return;

            lotteryContext.Gifts.Remove(gift);
            await lotteryContext.SaveChangesAsync();
        }

        public async Task<GiftModel> GetByName(string name)
        {
            return await this.lotteryContext.Gifts
                .FirstOrDefaultAsync(d => d.Name == name);
        }

        public async Task<GiftModel> GetByCategory(string category)
        {
            return await this.lotteryContext.Gifts
                .FirstOrDefaultAsync(d => d.Category == category);
        }

        public async Task<List<GiftModel>> GetByDonor(string name)
        {
            return await this.lotteryContext.Gifts
                .Include(g => g.Donor)
                .Where(g => (g.Donor.FirstName + " " + g.Donor.LastName).Contains(name))
                .ToListAsync();
        }

        public async Task<List<GiftModel>> GetByParches(int count)
        {
            return await lotteryContext.Gifts
                .Include(g => g.Donor)
                .Include(g => g.customerDatails)
                .Where(g => g.customerDatails.Count == count)
                .ToListAsync();
        }

        public async Task<List<GiftModel>> GetByPurchasesCount(int num)
        {
            return await lotteryContext.Gifts
                .Include(g => g.customerDatails)
                .Where(g => g.customerDatails != null &&
                            g.customerDatails.Sum(cd => cd.Quntity) == num)
                .ToListAsync();
        }

        public async Task<List<GiftModel>> giftExpensive()
        {
            return await this.lotteryContext.Gifts
                .OrderByDescending(g => g.PriceCard)
                .ToListAsync();
        }

        public async Task<GiftModel> byMostParches()
        {
            return await this.lotteryContext.Gifts
               .OrderByDescending(g => g.customerDatails.Count)
               .FirstOrDefaultAsync();
        }

        public async Task<GiftModel> Winner(int giftId)
        {
            return await lotteryContext.Gifts
                .Include(g => g.customerDatails)
                    .ThenInclude(cd => cd.customer)
                .FirstOrDefaultAsync(g => g.Id == giftId);
        }

        public async Task<List<GiftDto>> reportWinners()
        {
            return await lotteryContext.Gifts
                .Include(g => g.Winner)
                .Include(g => g.Donor)
                .Where(g => g.WinnerId != null && g.WinnerId != 0)
                .Select(g => new GiftDto
                {
                    Id = g.Id,
                    Name = g.Name,
                    PriceCard = g.PriceCard,
                    GiftImage = g.GiftImage,
                    Category = g.Category ?? "",
                    DonorId = g.DonorId,
                    DonorName = g.Donor != null ? g.Donor.FirstName + " " + g.Donor.LastName : "ללא תורם",
                    CustomerId = g.WinnerId ?? 0,
                    CustomerName = g.Winner != null ? g.Winner.FirstName + " " + g.Winner.LastName : "אין זוכה"
                })
                .ToListAsync();
        }

        public async Task<int> reportAchnasot()
        {
            return await lotteryContext.Gifts
                .SelectMany(g => g.customerDatails)
                .Where(g => g.Status != "Draft")
                .SumAsync(cd => cd.Quntity * cd.gift.PriceCard);
        }
    }
}