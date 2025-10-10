using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using Tuan06.Models;

namespace Tuan06.Controllers {
  public class CartController : Controller {
    private readonly string _connectionString;

    // Khai báo một hằng số cho UserID tạm thời
    private const int CurrentUserID = 1;

    public CartController(IConfiguration configuration) {
      _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    // 1. Phương thức Index: Thêm WHERE UserID
    public IActionResult Index() {
      List<Cart> carts = new List<Cart>();

      using (SqlConnection conn = new SqlConnection(_connectionString)) {
        string sql = @"
                     SELECT c.UserID, c.Quantity, 
                            p.ProductID, p.ProductName, p.Price, p.DiscountPrice, p.ProductImage
                     FROM Cart c
                     INNER JOIN Product p ON c.ProductID = p.ProductID
                     WHERE c.UserID = @UserID"; // LỌC GIỎ HÀNG THEO USER ID

        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@UserID", CurrentUserID); // Thêm tham số UserID
        conn.Open();

        SqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read()) {
          var product = new Product {
            ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
            ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
            Price = reader.GetDecimal(reader.GetOrdinal("Price")),
            DiscountPrice = reader.IsDBNull(reader.GetOrdinal("DiscountPrice")) ? 0 : reader.GetDecimal(reader.GetOrdinal("DiscountPrice")),
            ProductImage = reader.IsDBNull(reader.GetOrdinal("ProductImage")) ? "" : reader.GetString(reader.GetOrdinal("ProductImage"))
          };

          var cart = new Cart {
            UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
            Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
            Product = product
          };

          carts.Add(cart);
        }
        reader.Close();
      }

      decimal total = 0;
      foreach (var c in carts) {
        var price = c.Product.DiscountPrice > 0 ? c.Product.DiscountPrice : c.Product.Price;
        total += price * c.Quantity;
      }

      ViewBag.Total = total;
      return View(carts);
    }

    // 2. Phương thức Add: Bổ sung UserID vào truy vấn
    [HttpGet]
    public IActionResult Add(int id, int qty = 1) {
      using (SqlConnection conn = new SqlConnection(_connectionString)) {
        conn.Open();

        // Kiểm tra xem sản phẩm đã có trong giỏ hàng chưa (của USER hiện tại)
        string checkSql = "SELECT Quantity FROM Cart WHERE ProductID = @ProductID AND UserID = @UserID";
        SqlCommand checkCmd = new SqlCommand(checkSql, conn);
        checkCmd.Parameters.AddWithValue("@ProductID", id);
        checkCmd.Parameters.AddWithValue("@UserID", CurrentUserID); // LỌC THEO USER ID
        var existingQty = checkCmd.ExecuteScalar();

        if (existingQty != null) {
          // Nếu đã có -> cập nhật số lượng
          string updateSql = "UPDATE Cart SET Quantity = Quantity + @Qty WHERE ProductID = @ProductID AND UserID = @UserID";
          SqlCommand updateCmd = new SqlCommand(updateSql, conn);
          updateCmd.Parameters.AddWithValue("@Qty", qty);
          updateCmd.Parameters.AddWithValue("@ProductID", id);
          updateCmd.Parameters.AddWithValue("@UserID", CurrentUserID); // LỌC THEO USER ID
          updateCmd.ExecuteNonQuery();
        } else {
          // Nếu chưa có -> thêm mới (Bổ sung UserID)
          string insertSql = "INSERT INTO Cart (UserID, ProductID, Quantity, CreatedAt) VALUES (@UserID, @ProductID, @Qty, GETDATE())";
          SqlCommand insertCmd = new SqlCommand(insertSql, conn);
          insertCmd.Parameters.AddWithValue("@UserID", CurrentUserID); // THÊM USER ID
          insertCmd.Parameters.AddWithValue("@ProductID", id);
          insertCmd.Parameters.AddWithValue("@Qty", qty);
          insertCmd.ExecuteNonQuery();
        }
      }

      return RedirectToAction("Index");
    }

    // 3. Phương thức Update: Bổ sung UserID vào truy vấn
    [HttpPost]
    public IActionResult Update(int id, int qty) {
      if (qty < 1) qty = 1;

      using (SqlConnection conn = new SqlConnection(_connectionString)) {
        // Thêm UserID vào điều kiện WHERE
        string sql = "UPDATE Cart SET Quantity = @qty WHERE ProductID = @id AND UserID = @UserID";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@qty", qty);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@UserID", CurrentUserID); // Thêm tham số UserID
        conn.Open();
        cmd.ExecuteNonQuery();
      }

      return RedirectToAction("Index");
    }

    // 4. Phương thức Remove: Bổ sung UserID vào truy vấn
    [HttpPost]
    public IActionResult Remove(int id) {
      using (SqlConnection conn = new SqlConnection(_connectionString)) {
        // Thêm UserID vào điều kiện WHERE
        string sql = "DELETE FROM Cart WHERE ProductID = @id AND UserID = @UserID";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@UserID", CurrentUserID); // Thêm tham số UserID
        conn.Open();
        cmd.ExecuteNonQuery();
      }

      return RedirectToAction("Index");
    }
  }
}