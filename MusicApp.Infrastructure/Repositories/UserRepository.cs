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

        public UserRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if(user == null)
            {
                throw new Exception("User not found");
            }
            return user;
        }

        public async Task AddUserAsync(User user)
        {
            await _userManager.CreateAsync(user);
        }

        public async Task UpdateUserAsync(User user)
        {
            // if (string.IsNullOrEmpty(user.SecurityStamp))
            // {
            //      user.SecurityStamp = Guid.NewGuid().ToString();
            // }
            var existUser = await GetUserByIdAsync(user.Id);
            //_context.Entry(existUser).CurrentValues.SetValues(user);
            existUser.UserName = user.UserName;
            existUser.Email = user.Email;
            
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
            var users =  _userManager.Users;
            if (users == null)
            {
                throw new Exception("No users found");
            }
            return await users.ToListAsync();
        }
    }
}
