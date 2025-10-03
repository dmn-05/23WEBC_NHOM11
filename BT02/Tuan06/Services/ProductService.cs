using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Tuan06.Models;

namespace Tuan06.Services {
  public class ProductService : IProductService {
    private List<Product> _products;
    //middleware_Begin_Khai
    public ProductService() {
      // Đọc file db.json
      var jsonData = File.ReadAllText("db.json");

      // Parse JSON thành List<Product>
      _products = JsonSerializer.Deserialize<List<Product>>(jsonData);
    }

    public List<Product> GetAll() => _products;

    public Product GetById(int id) => _products.Find(p => p.MaSP == id);

    public void Add(Product p) => _products.Add(p);

    public void Update(Product p) {
      var index = _products.FindIndex(x => x.MaSP == p.MaSP);
      if (index != -1) _products[index] = p;
    }

    public void Delete(int id) {
      var product = _products.Find(p => p.MaSP == id);
      if (product != null) _products.Remove(product);
    }
  }
}
