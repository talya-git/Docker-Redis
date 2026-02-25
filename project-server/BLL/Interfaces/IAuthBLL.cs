using server.DTO;
using WebApplication1.Models;
using System.Threading.Tasks;

namespace server.BLL.Interfaces
{
    public interface IAuthBLL
    {
        Task Register(CustomerModel customerModel);
        Task<AuthDTO> Login(string username, string password);
    }
}