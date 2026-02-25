using WebApplication1.DAL.Interfaces;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication1.DAL
{
    public class CustomerDAL : IcustomerDAL
    {
        private readonly LotteryContext LotteryContext;

        public CustomerDAL(LotteryContext _lotteryContext)
        {
            this.LotteryContext = _lotteryContext;
        }

        public async Task<List<CustomerModel>> Get()
        {
            return await this.LotteryContext.Customers.ToListAsync();
        }

        public async Task<CustomerModel> GetById(int id)
        {
            return await LotteryContext.Customers.FirstOrDefaultAsync(d => d.Id == id);
        }
    }
}