namespace project.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

 
        public int RoleID { get; set; }

       
        public Role? Role { get; set; }
    }
}
