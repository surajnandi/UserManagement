using UserManagement.Models;

namespace UserManagement.Repository
{
    public interface IUsersRepository
    {
        Task<IEnumerable<Users>> GetAllUsers();
        Task<Users> GetUserById(int id);
        Task<Users> AddUser(Users user);
        Task<Users> UpdateUser(int id, Users user);
        Task<bool> DeleteUser(int id);
        Task<bool> IsUserNameUnique(string userName);
        Task<bool> IsEmailUnique(string email);
        Task<IEnumerable<Users>> GetActiveUsers();
        Task<IEnumerable<Users>> SearchUsersByEmail(string email);
        Task IsActive(int id);
    }
}
