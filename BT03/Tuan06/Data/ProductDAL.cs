using Tuan06.Models;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
namespace Tuan06.Data
{
    public class ProductDAL
    {
        private readonly string? _connectionString;
        public ProductDAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Product> GetAllProducts()
        {
            var products = new List<Product>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = "SELECT ProductId, ProductName, Price, DiscountPrice, ProductImage, ProductDescription FROM Product";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        ProductID = (int)reader["ProductID"],
                        ProductName = reader["ProductName"].ToString(),
                        Price = (decimal)reader["Price"],
                        DiscountPrice =(decimal)reader["DiscountPrice"],   
                        ProductImage = reader["ProductImage"].ToString(),
                        ProductDescription = reader["ProductDescription"].ToString(),
                    });
                }
            }
            return products;
        }
        public Product GetProductById(int id)
        {
            Product product = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = "SELECT ProductId, ProductName, Price, DiscountPrice, ProductImage, ProductDescription FROM PRODUCT WHERE ProductId = @ProductId";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductId", id);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            product = new Product
                            {
                                ProductID = (int)reader["ProductId"],
                                ProductName = reader["ProductName"].ToString(),
                                ProductDescription = reader["ProductDescription"].ToString(),
                                Price = (decimal)reader["Price"],
                                DiscountPrice = (decimal)reader["DiscountPrice"],
                                ProductImage = reader["ProductImage"].ToString()
                            };
                        }
                    }
                }
            }

            return product;
        }
    }
}
