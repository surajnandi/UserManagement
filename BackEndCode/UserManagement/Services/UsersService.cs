using UserManagement.Models;
using UserManagement.Repository;
using System.Text.RegularExpressions;

namespace UserManagement.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _repository;

        public UsersService (IUsersRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Users>> GetAllUsers()
        {
            return await _repository.GetAllUsers();
        }

        public async Task<Users> GetUserById(int id)
        {
            return await _repository.GetUserById(id);
        }

        public async Task<Users> AddUser(Users user)
        {
            ValidateUser(user);

            if (!await _repository.IsUserNameUnique(user.UserName))
            {
                throw new InvalidOperationException("Username already exists.");
            }

            if (!await _repository.IsEmailUnique(user.Email))
            {
                throw new InvalidOperationException("Email already exists.");
            }
            user.IsActive = true;
            user.CreateDate = DateTime.Now;
            return await _repository.AddUser(user);
        }

        public async Task<Users> UpdateUser(int id, Users user)
        {
            //ValidateUser(user);
            //return await _repository.UpdateUser(id, user);

            var existingUser = await _repository.GetUserById(id);

            if (existingUser == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            ValidateUser(user);

            existingUser.UserName = user.UserName;
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            existingUser.Phone = user.Phone;
            existingUser.Address = user.Address;
            existingUser.Password = user.Password;
            existingUser.CreateDate = existingUser.CreateDate;
            existingUser.UpdateDate = DateTime.Now;

            if (user.IsActive.HasValue)
            {
                existingUser.IsActive = user.IsActive.Value;
            }

            return await _repository.UpdateUser(id, existingUser);

        }

        public async Task<bool> DeleteUser(int id)
        {
            return await _repository.DeleteUser(id);
        }

        private void ValidateUser(Users user)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(user.UserName))
                errors.Add("Username is required.");

            if (string.IsNullOrWhiteSpace(user.FirstName))
                errors.Add("First name is required.");

            if (string.IsNullOrWhiteSpace(user.LastName))
                errors.Add("Last name is required.");

            if (string.IsNullOrWhiteSpace(user.Email))
                errors.Add("Email is required.");
            else if (!IsValidEmail(user.Email))
                errors.Add("Invalid email format.");

            if (string.IsNullOrWhiteSpace(user.Phone))
                errors.Add("Phone number is required.");
            else if (!IsNumeric(user.Phone))
                errors.Add("Phone number must be numeric.");

            if (string.IsNullOrWhiteSpace(user.Password))
                errors.Add("Password is required.");

            if (errors.Count > 0)
            {
                throw new ArgumentException(string.Join(" ", errors));
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, @"^[0-9]*$");
        }

        public Task<IEnumerable<Users>> GetActiveUsers()
        {
            return _repository.GetActiveUsers();
        }

        public Task<IEnumerable<Users>> SearchUsersByEmail(string email)
        {
            return _repository.SearchUsersByEmail(email);
        }

        public async Task IsActive(int id)
        {
            await _repository.IsActive(id);
        }


    }
}
