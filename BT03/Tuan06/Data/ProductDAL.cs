using Tuan06.Models;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.IO;
namespace Tuan06.Data {
  public class ProductDAL {
    private readonly string? _connectionString;
    public ProductDAL(IConfiguration configuration) {
      _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public string GetAllProductsJson() {
      var rows = new List<Dictionary<string, object>>();
      using (SqlConnection conn = new SqlConnection(_connectionString)) {
        string sql = "SELECT ProductID, ProductName, Price, Quantity, DiscountPrice, ProductImage ,ProductDescription, Category.CategoryID, Category.CategoryName FROM [PRODUCT] join [CATEGORY] on Product.CategoryID=Category.CategoryID";
        SqlCommand cmd = new SqlCommand(sql, conn);
        conn.Open();

        SqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read()) {
          var row = new Dictionary<string, object>();
          for (int i = 0; i < reader.FieldCount; i++) {
            row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
          }
          rows.Add(row);
        }
      }
      return JsonSerializer.Serialize(rows);
    }

    public void SaveProductsToJsonFile() {
      // Lấy dữ liệu JSON từ database
      string jsonArray = GetAllProductsJson();

      // Tạo object có thuộc tính "products"
      var wrapper = new {
        products = JsonSerializer.Deserialize<object>(jsonArray)
      };

      // Chuyển object đó lại thành chuỗi JSON đẹp (indented)
      string fullJson = JsonSerializer.Serialize(wrapper, new JsonSerializerOptions {
        WriteIndented = true
      });

      // Ghi ra file db.json
      string filePath = Path.Combine(Directory.GetCurrentDirectory(), "db.json");

      // Tạo thư mục nếu chưa có
      Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

      File.WriteAllText(filePath, fullJson);
    }

    public List<Dictionary<string, object>> GetAllProducts() {
      SaveProductsToJsonFile();
      // Đường dẫn đến file JSON
      string filePath = Path.Combine(Directory.GetCurrentDirectory(), "db.json");
      if (!File.Exists(filePath)) {
        return new List<Dictionary<string, object>>();// Trả về rỗng nếu file chưa tồn tại
      }
      // Đọc toàn bộ nội dung file JSON
      string jsonContent = File.ReadAllText(filePath);
      // Giải mã file JSON
      using var doc = JsonDocument.Parse(jsonContent);

      // Lấy phần tử "products" trong JSON
      if (doc.RootElement.TryGetProperty("products", out JsonElement productsElement)) {
        // Giải mã "products" thành List<Dictionary<string, object>>
        var products = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(productsElement.GetRawText());
        return products ?? new List<Dictionary<string, object>>();
      }

      // Nếu không có "products" thì trả rỗng
      return new List<Dictionary<string, object>>();
    }

    public string GetProductIDJson(int id) {
      var row = new Dictionary<string, object>();

      using (SqlConnection conn = new SqlConnection(_connectionString)) {
        string sql = "SELECT ProductID, ProductName, Price, Quantity, DiscountPrice, ProductImage, ProductDescription, Category.CategoryID, Category.CategoryName FROM [PRODUCT] JOIN [CATEGORY] ON Product.CategoryID = Category.CategoryID WHERE ProductId = @ProductId";

        using (SqlCommand cmd = new SqlCommand(sql, conn)) {
          cmd.Parameters.AddWithValue("@ProductId", id);
          conn.Open();

          using (SqlDataReader reader = cmd.ExecuteReader()) {
            if (reader.Read()) { // thêm điều kiện đọc dòng
              for (int i = 0; i < reader.FieldCount; i++) {
                row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
              }
            }
          }
        }
      }

      return JsonSerializer.Serialize(row);
    }


    public void SaveProductIDToJsonFile(int id) {
      string filePath = Path.Combine(Directory.GetCurrentDirectory(), "db.json");

      // Lấy dữ liệu JSON từ database
      string jsonArray = GetProductIDJson(id);
      var newProductID = JsonSerializer.Deserialize<object>(jsonArray);

      object? productsData = null;

      if (File.Exists(filePath)) {
        string oldJson = File.ReadAllText(filePath);
        using var oldDoc = JsonDocument.Parse(oldJson);
        if (oldDoc.RootElement.TryGetProperty("products", out JsonElement productsElement)) {
          productsData = JsonSerializer.Deserialize<object>(productsElement.GetRawText());
        }
      }

      var wrapper = new {
        products = productsData ?? new List<object>(),
        productID = newProductID
      };

      string fullJson = JsonSerializer.Serialize(wrapper, new JsonSerializerOptions { WriteIndented = true });
      File.WriteAllText(filePath, fullJson);
    }



    public void ClearProductIDData() {
      // Đường dẫn file db.json
      string filePath = Path.Combine(Directory.GetCurrentDirectory(), "db.json");

      // Nếu file chưa tồn tại thì tạo mới
      if (!File.Exists(filePath)) {
        var newJson = new {
          products = new List<object>(),
          productID = new object() // giữ key productID
        };
        string newJsonContent = JsonSerializer.Serialize(newJson, new JsonSerializerOptions {
          WriteIndented = true
        });
        File.WriteAllText(filePath, newJsonContent);
        return;
      }

      // Đọc nội dung JSON hiện tại
      string jsonContent = File.ReadAllText(filePath);
      using var doc = JsonDocument.Parse(jsonContent);

      // Tạo đối tượng lưu dữ liệu mới
      object? productsData = null;

      // Giữ lại phần products nếu có
      if (doc.RootElement.TryGetProperty("products", out JsonElement productsElement)) {
        productsData = JsonSerializer.Deserialize<object>(productsElement.GetRawText());
      }

      // Ghi lại file nhưng productID rỗng
      var newData = new {
        products = productsData ?? new List<object>(),
        productID = new object() // làm rỗng productID
      };

      string updatedJson = JsonSerializer.Serialize(newData, new JsonSerializerOptions {
        WriteIndented = true
      });

      File.WriteAllText(filePath, updatedJson);
    }

    public Dictionary<string, object> GetProductById(int id) {
      //Xóa dữ liệu cũ trong productID trong db.json
      ClearProductIDData();
      //Đổ dữ liệu vô productID
      SaveProductIDToJsonFile(id);
      // Đường dẫn đến file JSON
      string filePath = Path.Combine(Directory.GetCurrentDirectory(), "db.json");
      if (!File.Exists(filePath)) {
        return new Dictionary<string, object>();// Trả về rỗng nếu file chưa tồn tại
      }
      // Đọc toàn bộ nội dung file JSON
      string jsonContent = File.ReadAllText(filePath);
      // Giải mã file JSON
      using var doc = JsonDocument.Parse(jsonContent);

      // Lấy phần tử "productID" trong JSON
      if (doc.RootElement.TryGetProperty("productID", out JsonElement productsElement)) {
        // Giải mã "products" thành List<Dictionary<string, object>>
        var productID = JsonSerializer.Deserialize<Dictionary<string, object>>(productsElement.GetRawText());
        return productID ?? new Dictionary<string, object>();
      }

      // Nếu không có "products" thì trả rỗng
      return new Dictionary<string, object>();
    }

    public List<Dictionary<string, object>> GetProductsPagedFromJson(int page, int pageSize, out int totalProducts) {
      totalProducts = 0;

      // Đường dẫn tới file db.json
      string filePath = Path.Combine(Directory.GetCurrentDirectory(), "db.json");
      if (!File.Exists(filePath)) {
        return new List<Dictionary<string, object>>();
      }

      // Đọc toàn bộ JSON
      string jsonContent = File.ReadAllText(filePath);
      using var doc = JsonDocument.Parse(jsonContent);

      // Lấy mảng "products"
      if (!doc.RootElement.TryGetProperty("products", out JsonElement productsElement)) {
        return new List<Dictionary<string, object>>();
      }

      // Chuyển thành list dictionary
      var allProducts = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(productsElement.GetRawText())
                        ?? new List<Dictionary<string, object>>();

      // Tổng số sản phẩm
      totalProducts = allProducts.Count;

      // Phân trang (page bắt đầu từ 1)
      int skip = (page - 1) * pageSize;
      var pagedProducts = allProducts.Skip(skip).Take(pageSize).ToList();

      return pagedProducts;
    }
    public void AddProduct(Product product) {
      using (SqlConnection conn = new SqlConnection(_connectionString)) {
        string sql = "INSERT INTO [PRODUCT] (ProductName, Price, DiscountPrice, ProductImage, Quantity, ProductDescription, CategoryID) VALUES (@ProductName, @Price, @DiscountPrice, @ProductImage, @Quantity, @ProductDescription, @CategoryID)";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
        cmd.Parameters.AddWithValue("@Price", product.Price);
        cmd.Parameters.AddWithValue("@DiscountPrice", product.DiscountPrice);
        cmd.Parameters.AddWithValue("@ProductImage", product.ProductImage);
        cmd.Parameters.AddWithValue("@ProductDescription", product.ProductDescription);
        cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
        cmd.Parameters.AddWithValue("@CategoryID", product.CategoryID);
        conn.Open();
        cmd.ExecuteNonQuery();
      }
    }
  }
}
