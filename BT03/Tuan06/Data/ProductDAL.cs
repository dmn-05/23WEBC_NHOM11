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
                string sql = "SELECT ProductID, ProductName, Price, DiscountPrice, ProductImage, ProductDescription, Categories.CategoryID, Categories.CategoryName FROM Products join Categories on Products.CategoryID=Categories.CategoryID";
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
                        DiscountPrice = (decimal)reader["DiscountPrice"],
                        ProductImage = reader["ProductImage"].ToString(),
                        ProductDescription = reader["ProductDescription"].ToString(),
                        CategoryID = (int)reader["CategoryID"],
                        CategoryName = reader["CategoryName"].ToString()
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
                string sql = "SELECT ProductID, ProductName, Price, DiscountPrice, ProductImage, ProductDescription, Categories.CategoryID, Categories.CategoryName FROM Products join Categories on Products.CategoryID=Categories.CategoryID WHERE ProductId = @ProductId";

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
                                ProductID = (int)reader["ProductID"],
                                ProductName = reader["ProductName"].ToString(),
                                ProductDescription = reader["ProductDescription"].ToString(),
                                Price = (decimal)reader["Price"],
                                DiscountPrice = (decimal)reader["DiscountPrice"],
                                ProductImage = reader["ProductImage"].ToString(),
                                CategoryID = (int)reader["CategoryID"],
                                CategoryName = reader["CategoryName"].ToString()
                            };
                        }
                    }
                }
            }

            return product;
        }
        public void AddProduct(Product product)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = "INSERT INTO Products (ProductName, Price, DiscountPrice, ProductImage, ProductDescription, CategoryID) VALUES (@ProductName, @Price, @DiscountPrice, @ProductImage, @ProductDescription, @CategoryID)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@DiscountPrice", product.DiscountPrice);
                cmd.Parameters.AddWithValue("@ProductImage", product.ProductImage);
                cmd.Parameters.AddWithValue("@ProductDescription", product.ProductDescription);
                cmd.Parameters.AddWithValue("@CategoryID", product.CategoryID);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
