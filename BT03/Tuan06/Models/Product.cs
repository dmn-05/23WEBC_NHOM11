using Microsoft.AspNetCore.Mvc;

namespace Tuan06.Models {
  public class Product {
    public int ProductID{ get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public decimal DiscountPrice { get; set; }
    public string ProductImage { get; set; }
    public string ProductDescription { get; set; }
    public int CategoryID { get; set; }
    public string CategoryName { get; set; }
    }
}

