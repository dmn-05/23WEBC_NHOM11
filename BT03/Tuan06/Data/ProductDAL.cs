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
                string sql = @"SELECT ProductID, ProductName, ProductImage, CategoryID, Price, DiscountPrice, Quantity, ProductDescription, ProductStatus FROM Product";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        ProductID = (int)reader["ProductID"],
                        ProductName = reader["ProductName"].ToString(),
                        ProductImage = reader["ProductImage"].ToString(),
                        CategoryID = (int)reader["CategoryID"],
                        Price = (decimal)reader["Price"],
                        DiscountPrice = (decimal)reader["DiscountPrice"],
                        Quantity = (int)reader["Quantity"],
                        ProductDescription = reader["ProductDescription"].ToString(),
                        ProductStatus = reader["ProductStatus"] != DBNull.Value ? Convert.ToBoolean(reader["ProductStatus"]) : false
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

        public List<Category> GetAllCategories()
        {
            var categories = new List<Category>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = "SELECT CategoryID, CategoryName, CategoryDescription, CategoryStatus FROM Category";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    categories.Add(new Category
                    {
                        CategoryID = (int)reader["CategoryID"],
                        CategoryName = reader["CategoryName"].ToString(),
                        CategoryDescription = reader["CategoryDescription"].ToString(),
                        CategoryStatus = reader["CategoryStatus"] != DBNull.Value ? Convert.ToBoolean(reader["CategoryStatus"]) : false
                    });
                }
            }
            return categories;
        }

        public void AddProduct(Product product)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"INSERT INTO Product 
                    (ProductName, ProductImage, CategoryID, Price, DiscountPrice, Quantity, ProductDescription, ProductStatus)
                    VALUES (@ProductName, @ProductImage, @CategoryID, @Price, @DiscountPrice, @Quantity, @ProductDescription, @ProductStatus)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductName", product.ProductName ?? "");
                    cmd.Parameters.AddWithValue("@ProductImage", product.ProductImage ?? "");
                    cmd.Parameters.AddWithValue("@CategoryID", product.CategoryID);
                    cmd.Parameters.AddWithValue("@Price", product.Price);
                    cmd.Parameters.AddWithValue("@DiscountPrice", product.DiscountPrice);
                    cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
                    cmd.Parameters.AddWithValue("@ProductDescription", product.ProductDescription ?? "");
                    cmd.Parameters.AddWithValue("@ProductStatus", product.ProductStatus);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void AddCategory(Category category)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"INSERT INTO Category (CategoryName, CategoryDescription, CategoryStatus)
                               VALUES (@CategoryName, @CategoryDescription, @CategoryStatus)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@CategoryName", category.CategoryName ?? "");
                    cmd.Parameters.AddWithValue("@CategoryDescription", category.CategoryDescription ?? "");
                    cmd.Parameters.AddWithValue("@CategoryStatus", category.CategoryStatus);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
