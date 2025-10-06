using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using Tuan06.Models;

namespace Tuan06.Services {
  public class ProductService : IProductService {
    private readonly string _filePath = "db.json";
    private List<Product> _products;

    public ProductService() {
      // Nếu file chưa có thì tạo mới với mảng rỗng
      if (!File.Exists(_filePath)) {
        File.WriteAllText(_filePath, "{\"products\":[]}");
      }

      // Đọc dữ liệu từ db.json
      var jsonData = File.ReadAllText(_filePath);
      var jsonObject = JsonNode.Parse(jsonData);
      _products = jsonObject?["products"]?.Deserialize<List<Product>>() ?? new List<Product>();
    }

    public List<Product> GetAll() => _products;

    public Product GetById(int id) => _products.FirstOrDefault(p => p.MaSP == id);

    public void Add(Product p) {
      p.MaSP = _products.Any() ? _products.Max(x => x.MaSP) + 1 : 1;
      _products.Add(p);
      SaveChanges();
    }

    public void Update(Product p) {
      var index = _products.FindIndex(x => x.MaSP == p.MaSP);
      if (index != -1) {
        _products[index] = p;
        SaveChanges();
      }
    }

    public void Delete(int id) {
      var product = _products.FirstOrDefault(p => p.MaSP == id);
      if (product != null) {
        _products.Remove(product);
        SaveChanges();
      }
    }

    private void SaveChanges() {
      var options = new JsonSerializerOptions { WriteIndented = true };
      var jsonObject = new { products = _products };
      var jsonData = JsonSerializer.Serialize(jsonObject, options);
      File.WriteAllText(_filePath, jsonData);
    }
  }
}
