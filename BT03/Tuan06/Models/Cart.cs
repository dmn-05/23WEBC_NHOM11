namespace Tuan06.Models
{
  public class Cart
  {
    public int UserID { get; set; }
    public int ProductID { get; set; }
    public decimal Quantity { get; set; }
    public DateTime CreatedAt { get; set; }

    // Các thuộc tính khác 

    public string ProductName { get ; set; }    


    }
}
