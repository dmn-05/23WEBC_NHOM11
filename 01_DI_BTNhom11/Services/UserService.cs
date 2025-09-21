namespace _01_DI_BTNhom11.Services
{
    public class UserService : IUserService
    {
        private List<User> _users;

        public List<User> GetUsers()
        {
            return _users ?? new List<User>();
        }

        public void SetUsers(List<User> users)
        {
            _users = users;
        }
    }

}
