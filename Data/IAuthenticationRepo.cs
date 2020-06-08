using System.Threading.Tasks;
using HomeschoolHelperApi.DTOs.Users;
using HomeschoolHelperApi.Models;

namespace HomeschoolHelperApi.Data
{
    public interface IAuthenticationRepo
    {
         Task<ServerResponse<int>> Register(User user, string password);
         Task<ServerResponse<string>> Login(string email, string password);
         Task<bool> UserExists(string email);

    }
}