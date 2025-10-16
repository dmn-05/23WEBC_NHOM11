using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using Tuan06.Models;
namespace Tuan06.Data
{
    public class CategoryDAL {
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
                string sql = "SELECT CategoryID, CategoryName FROM [CATEGORY]";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    categories.Add(new Category
                    {
                        CategoryID = (int)reader["CategoryID"],
                        CategoryName = reader["CategoryName"].ToString()
                    });
                }
            }

            return categories;
        }

        public void AddCategory(string categoryName)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = "INSERT INTO [CATEGORY] (CategoryName) VALUES (@CategoryName)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@CategoryName", categoryName);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}

