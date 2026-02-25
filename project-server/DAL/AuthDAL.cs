using server.DAL.Interfaces;
using WebApplication1.DAL;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace server.DAL
{
    public class AuthDAL : IAuthDAL
    {
        private readonly LotteryContext lotteryContext;

        public AuthDAL(LotteryContext lotteryContext)
        {
            this.lotteryContext = lotteryContext;
        }

        public async Task Register(CustomerModel customerModel)
        {
            await this.lotteryContext.Customers.AddAsync(customerModel);
            await this.lotteryContext.SaveChangesAsync();
        }

        public async Task<CustomerModel?> Login(string userName, string password)
        {

            var result = await this.lotteryContext.Customers
                .FirstOrDefaultAsync(d => d.Password.Trim() == password.Trim() && d.UserName.Trim() == userName.Trim());

            return result;
        }
        public async Task<CustomerModel> GetUserByName(string userName)
        {
            return await this.lotteryContext.Customers
                .FirstOrDefaultAsync(u => u.UserName == userName);
        }
    }
}