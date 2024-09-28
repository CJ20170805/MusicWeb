using Microsoft.AspNetCore.Identity;
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
            await _userManager.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            var user = await GetUserByIdAsync(userId);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
        }
    }
}
