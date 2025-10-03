using Microsoft.AspNetCore.Mvc;
using Tuan06.Models;
namespace Tuan06.Services {
  public interface IProductService {
    List<Product> GetAll();
    Product GetById(int id);
    void Add(Product p);
    void Update(Product p);
    void Delete(int id);
  }
}
