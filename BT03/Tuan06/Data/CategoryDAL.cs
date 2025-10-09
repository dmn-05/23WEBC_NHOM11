using Tuan06.Models;
using Microsoft.Data.SqlClient;
namespace Tuan06.Data
{
    public class CategoryDAL
    {
        private readonly string? _connectionString;
        public CategoryDAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public List<Category> GetAllCategories()
        {
            var categories = new List<Category>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = "SELECT CategoryID, CategoryName,CategoryDescription FROM Categories";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    categories.Add(new Category
                    {
                        CategoryID = (int)reader["CategoryID"],
                        CategoryName = reader["CategoryName"].ToString(),
                        CategoryDescription = reader["CategoryDescription"].ToString()

                    });
                }
            }
            return categories;
        }
    }
}
