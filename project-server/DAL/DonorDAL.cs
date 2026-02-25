using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.DAL.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.DAL
{
    public class DonorDAL : IdonorDAL
    {
        private readonly LotteryContext lotteryContext;

        public DonorDAL(LotteryContext lotteryContext)
        {
            this.lotteryContext = lotteryContext;
        }

        public async Task<List<DonorModel>> Get()
        {
            return await lotteryContext.Donors
                           .Include(d => d.Gifts)
                           .ToListAsync();
        }

        public async Task<DonorModel> GetById(int id)
        {
            return await lotteryContext.Donors.FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task Post(DonorModel donorModel)
        {
            await this.lotteryContext.Donors.AddAsync(donorModel);
            await this.lotteryContext.SaveChangesAsync();
        }

        public async Task Update(int id, DonorModel donorFromClient)
        {
            var donorFromDb = await lotteryContext.Donors.FindAsync(id);
            if (donorFromDb == null)
            {
                return;
            }

            donorFromDb.FirstName = donorFromClient.FirstName;
            donorFromDb.LastName = donorFromClient.LastName;
            donorFromDb.Email = donorFromClient.Email;
            donorFromDb.Phone = donorFromClient.Phone;
            donorFromDb.Address = donorFromClient.Address;

            await lotteryContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var donor = await lotteryContext.Donors.FindAsync(id);

            if (donor == null)
                return;

            lotteryContext.Donors.Remove(donor);
            await lotteryContext.SaveChangesAsync();
        }

        public async Task<DonorModel> GetByGmail(string email)
        {
            return await this.lotteryContext.Donors
                .FirstOrDefaultAsync(d => d.Email == email);
        }

        public async Task<DonorModel> GetByName(string name)
        {
            return await this.lotteryContext.Donors
                .FirstOrDefaultAsync(d => (d.FirstName + " " + d.LastName) == name);
        }

        public async Task<List<DonorModel>> GetByGift([FromQuery] string gift)
        {
            return await lotteryContext.Donors
                .Include(d => d.Gifts)
                .Where(d => d.Gifts.Any(g => g.Name.Contains(gift)))
                .ToListAsync();
        }
    }
}