using Microsoft.AspNetCore.Mvc;

namespace Tuan06.Models {
  public class Product {
		public int ProductID { get; set; }
		public string ProductName { get; set; }
		public string ProductImage { get; set; }
		public int CategoryID { get; set; }
		public decimal Price { get; set; }
		public decimal DiscountPrice { get; set; }
		public int Quantity { get; set; }
		public string ProductDescription { get; set; }
		public bool ProductStatus { get; set; }
	}
}

