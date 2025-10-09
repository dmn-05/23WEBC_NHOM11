namespace Tuan06.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string UserPassword { get; set; } 
        public string Phone { get; set; }
        public string Email { get; set; }
        public string UserAddress { get; set; }
        public int UserRole { get; set; }
        public bool UserStatus { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
