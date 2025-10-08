namespace Tuan06.Models
{
    public class Contact
    {
        public int ContactID { get; set; }
        public string FullName { get; set; }
        public string Content { get; set; } 
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool ContactStatus { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
