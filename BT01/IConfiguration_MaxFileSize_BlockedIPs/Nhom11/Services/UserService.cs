using Nhom11.Models;
using System.Collections.Generic;
using System.Linq;

namespace Nhom11.Services
{
    public class UserService : IUserService
    {
        private readonly List<User> _users;

        public UserService()
        {
           
            _users = new List<User>
            {
                new User { Username = "admin", Password = "123", Role = 1 },
                new User { Username = "manager", Password = "123", Role = 2 },
                new User { Username = "staff", Password = "123", Role = 3 }
            };
        }

        public List<User> GetAllUsers() => _users;

        public User? GetUser(string username) =>
            _users.FirstOrDefault(u => u.Username == username);
    }
}
