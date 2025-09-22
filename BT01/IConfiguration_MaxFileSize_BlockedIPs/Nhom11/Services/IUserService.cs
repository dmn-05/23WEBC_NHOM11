using Nhom11.Models;

namespace Nhom11.Services
{
    //Khai 21/09/2025
    public interface IUserService
    {
        List<User> GetAllUsers();
        User? GetUser(string username);
    }
}
