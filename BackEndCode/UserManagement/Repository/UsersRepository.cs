using Microsoft.EntityFrameworkCore;
using UserManagement.Data;
using UserManagement.Models;

namespace UserManagement.Repository
{
    public class UsersRepository : IUsersRepository
    {
        private readonly UsersContext _context;

        public UsersRepository(UsersContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Users>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<Users> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<Users> AddUser(Users user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<Users> UpdateUser(int id, Users user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsUserNameUnique(string userName)
        {
            return await _context.Users.AllAsync(c => c.UserName != userName);
        }

        public async Task<bool> IsEmailUnique(string email)
        {
            return await _context.Users.AllAsync(c => c.Email != email);
        }
        public async Task<IEnumerable<Users>> GetActiveUsers()
        {
            return await _context.Users.Where(c => c.IsActive == true).ToListAsync();
        }

        public async Task<IEnumerable<Users>> SearchUsersByEmail(string email)
        {
            return await _context.Users
                .Where(c => c.Email.Contains(email))
                .ToListAsync();
        }

        public async Task IsActive(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.IsActive = !user.IsActive;
            await _context.SaveChangesAsync();
        }

    }
}
