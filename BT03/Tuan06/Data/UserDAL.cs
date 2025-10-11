using Tuan06.Models;
using Microsoft.Data.SqlClient;
namespace Tuan06.Data
{
    public class UserDAL
    {
       private readonly string? _connectionString;

        public UserDAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public User?Login(string username, string password)
        {
            User? user = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Users WHERE Username = @Username AND UserPassword = @Password";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    user = new User()
                    {
                        UserID = (int)reader["UserID"],
                        UserName= reader["UserName"].ToString(),
                        FullName = reader["FullName"].ToString(),
                        UserPassword = reader["UserPassword"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        Email = reader["Email"].ToString(),
                        UserAddress = reader["UserAddress"].ToString(),
                        UserRole = (int)reader["UserRole"],
                        UserStatus = (bool)reader["UserStatus"],
                        CreatedAt = (DateTime)reader["CreatedAt"]
                    };
                    reader.Close();
                    return user;
                }
                reader.Close();
            }
            return null;
        }
        public List<Models.User> GetAllUsers()
        {
            List<Models.User> users = new List<Models.User>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Users";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Models.User user = new Models.User
                    {
                        UserID = (int)reader["UserID"],
                        FullName = reader["FullName"].ToString(),
                        UserPassword = reader["UserPassword"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        Email = reader["Email"].ToString(),
                        UserAddress = reader["UserAddress"].ToString(),
                        UserRole = (int)reader["UserRole"],
                        UserStatus = (bool)reader["UserStatus"],
                        CreatedAt = (DateTime)reader["CreatedAt"]
                    };
                    users.Add(user);
                }
                reader.Close();
            }
            return users;
        }
    }
}
