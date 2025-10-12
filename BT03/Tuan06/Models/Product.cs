
using System.ComponentModel.DataAnnotations;

namespace Tuan06.Models
{
    public class Product
    {
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Tên sản phẩm không được vượt quá 100 ký tự.")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Giá là bắt buộc.")]
        [Range(1000, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 1.000.")]
        public decimal Price { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá khuyến mãi không hợp lệ.")]
        public decimal DiscountPrice { get; set; }

        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự.")]
        public string? ProductDescription { get; set; }

        public string? ProductImage { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số lượng không hợp lệ.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn danh mục.")]
        public int CategoryID { get; set; }

        public string? CategoryName { get; set; }
    }
}
