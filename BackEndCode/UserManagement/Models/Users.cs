namespace UserManagement.Models
{
    public class Users
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string Password { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
