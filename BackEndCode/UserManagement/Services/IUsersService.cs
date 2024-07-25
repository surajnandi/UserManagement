using UserManagement.Models;

namespace UserManagement.Services
{
    public interface IUsersService
    {
        Task<IEnumerable<Users>> GetAllUsers();
        Task<Users> GetUserById(int id);
        Task<Users> AddUser(Users user);
        Task<Users> UpdateUser(int id, Users user);
        Task<bool> DeleteUser(int id);
        Task<IEnumerable<Users>> GetActiveUsers();
        Task<IEnumerable<Users>> SearchUsersByEmail(string email);
        Task IsActive(int id);
    }
}
