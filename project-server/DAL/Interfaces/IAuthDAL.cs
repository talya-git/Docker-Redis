using System.Threading.Tasks;
using WebApplication1.Models;

namespace server.DAL.Interfaces
{
    public interface IAuthDAL
    {
        Task Register(CustomerModel customerModel);

        Task<CustomerModel?> Login(string userName, string password);
        Task<CustomerModel> GetUserByName(string userName);
    }
}