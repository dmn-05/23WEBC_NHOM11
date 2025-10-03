using Microsoft.AspNetCore.Mvc;

namespace Tuan06.Models {
  public class Product {
    public int MaSP { get; set; }
    public string TenSP { get; set; }
    public decimal DonGia { get; set; }
    public decimal DonGiaKhuyenMai { get; set; }
    public string HinhAnh { get; set; }
    public string MoTa { get; set; }
    public string LoaiSP { get; set; }
  }
}

