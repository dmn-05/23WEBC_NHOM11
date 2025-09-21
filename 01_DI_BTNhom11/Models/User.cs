public class User
{
    public string Username { get; set; }
    public string Password { get; set; }
    public int Role { get; set; } // 1 = Admin, 2 = Editor, 3 = Viewer
}
