using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MusicApp.Domain.Entities;
using MusicApp.Domain.Interfaces;
using System.Threading.Tasks;

namespace MusicApp.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public UserRepository(UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new Exception("User not found");
            }
            return user;
        }

        public async Task<User> AddUserAsync(User user, string password)
        {
             
            Console.WriteLine("hohouuu" + user.Id +  user.UserName + user.Email  + password);
            var  result = await _userManager.CreateAsync(user, password);
             if (result.Succeeded)
            {
                Console.WriteLine("hohouuu-Yes" + user.UserName + user.Email  + password);
                return user;
            }
            else
            {
                throw new Exception($"Failed to create user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
            
        }

        public async Task UpdateUserAsync(User user)
        {
            var existUser = await GetUserByIdAsync(user.Id);
            existUser.UserName = user.UserName;
            existUser.Email = user.Email;

           // existUser.UserRoles.Clear();
            
            await _userManager.UpdateAsync(existUser);
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            var user = await GetUserByIdAsync(userId);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            if (users == null)
            {
                throw new Exception("No users found");
            }
            return users;
        }

        // Retrieve RoleId based on the role name
        private async Task<Guid> GetRoleIdByNameAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                throw new Exception($"Role '{roleName}' not found.");
            }
            return role.Id;
        }
    }
}
