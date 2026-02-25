using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebApplication1.Models;

namespace WebApplication1.DAL
{
    public class LotteryContext:DbContext
    {
        public DbSet<GiftModel> Gifts { get; set; }
        public DbSet<CustomerModel> Customers { get; set; }
        public DbSet<DonorModel> Donors { get; set; }

        public DbSet<CustomerDatails> CustomerDetails { get; set; }
        public LotteryContext(DbContextOptions<LotteryContext> options) : base(options)
        {

        }
    }
}
