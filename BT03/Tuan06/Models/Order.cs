namespace Tuan06.Models
{
  public class Order
  {
    public int OrderID { get; set; }
    public int UserID { get; set; }
    public decimal TotalAmount { get; set; }
    public bool OrderStatus { get; set; }
    public DateTime CreatedAt { get; set; }
  }
}
