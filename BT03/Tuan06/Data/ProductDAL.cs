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
                string sql = "SELECT ProductId, ProductName, Price, DiscountPrice, ProductImage, ProductDescription FROM Products";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        MaSP = (int)reader["ProductID"],
                        TenSP = reader["ProductName"].ToString(),
                        DonGia = (decimal)reader["Price"],
                        DonGiaKhuyenMai =(decimal)reader["DiscountPrice"],   
                        HinhAnh = reader["ProductImage"].ToString(),
                        MoTa = reader["ProductDescription"].ToString(),
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
                string sql = "SELECT ProductId, ProductName, Price, DiscountPrice, ProductImage, ProductDescription FROM PRODUCTS WHERE ProductId = @ProductId";

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
                                MaSP = (int)reader["ProductId"],
                                TenSP = reader["ProductName"].ToString(),
                                MoTa = reader["ProductDescription"].ToString(),
                                DonGia = (decimal)reader["Price"],
                                DonGiaKhuyenMai = (decimal)reader["DiscountPrice"],
                                HinhAnh = reader["ProductImage"].ToString()
                            };
                        }
                    }
                }
            }

            return product;
        }
    }
}
