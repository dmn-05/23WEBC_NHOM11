namespace Tuan06.Models
{
    public class CartItem
    {
        public int MaSP { get; set; }
        public string TenSP { get; set; } = "";
        public string HinhAnh { get; set; } = "";
        public decimal DonGia { get; set; }
        public decimal DonGiaKhuyenMai { get; set; }
        public int Qty { get; set; } = 1;
        public decimal SubTotal => (DonGiaKhuyenMai > 0 ? DonGiaKhuyenMai : DonGia) * Qty;
    }
}
