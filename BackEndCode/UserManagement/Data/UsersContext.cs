using Microsoft.EntityFrameworkCore;
using UserManagement.Models;

namespace UserManagement.Data
{
    public class UsersContext : DbContext
    {
        public UsersContext(DbContextOptions<UsersContext> options)
            : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
    }
}
