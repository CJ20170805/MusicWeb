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

        public async Task AddUserAsync(User user)
        {
            await _userManager.CreateAsync(user);
        }

        public async Task UpdateUserAsync(User user)
        {
            var existUser = await GetUserByIdAsync(user.Id);
            existUser.UserName = user.UserName;
            existUser.Email = user.Email;

            existUser.UserRoles.Clear();
            
            await _userManager.UpdateAsync(existUser);

            // Retrieve the existing user from the database
            // var existUser = await GetUserByIdAsync(user.Id);
            // if (existUser == null)
            // {
            //     throw new Exception("User not found.");
            // }

            // Update user properties
            // existUser.UserName = user.UserName;
            // existUser.Email = user.Email;
            // existUser.SecurityStamp = Guid.NewGuid().ToString();

            // // Get existing roles
            // var existingRoles = await _userManager.GetRolesAsync(existUser);

            // // Retrieve all roles to map RoleId to RoleName
            // var allRoles = await _roleManager.Roles.ToListAsync();
            // var roleIdToNameMap = allRoles.ToDictionary(role => role.Id.ToString(), role => role.Name);

            // // Extract new role names based on the role IDs from user.UserRoles
            // var newRoleNames = user.UserRoles
            //     .Select(ur => roleIdToNameMap.TryGetValue(ur.RoleId.ToString(), out var roleName) ? roleName : null)
            //     .Where(roleName => roleName != null) // Ensure to filter out nulls for unmatched RoleId
            //     .Cast<string>() // Cast to string to remove nullable reference
            //     .ToList();

            // // Check if the roles have changed
            // if (!existingRoles.SequenceEqual(newRoleNames))
            // {
            //     Console.WriteLine("Updating roles for user: " + existUser.UserName);

            //     // Determine roles to remove and add
            //     var rolesToRemove = existingRoles.Except(newRoleNames).ToList();
            //     var rolesToAdd = newRoleNames.Except(existingRoles).ToList();

            //     // Remove roles that are no longer assigned
            //     if (rolesToRemove.Any())
            //     {
            //         var removeResult = await _userManager.RemoveFromRolesAsync(existUser, rolesToRemove);
            //         if (!removeResult.Succeeded)
            //         {
            //             throw new InvalidOperationException("Failed to remove roles from user.");
            //         }
            //         Console.WriteLine("Removed roles: " + string.Join(", ", rolesToRemove));
            //     }

            //     // Add new roles only if they are not already assigned
            //     if (rolesToAdd.Any())
            //     {
            //         var addResult = await _userManager.AddToRolesAsync(existUser, rolesToAdd);
            //         if (!addResult.Succeeded)
            //         {
            //             throw new InvalidOperationException("Failed to add roles to user.");
            //         }
            //         Console.WriteLine("Added roles: " + string.Join(", ", rolesToAdd));
            //     }
            // }


            // existUser.UserRoles.Clear();

            // // Update the user entity
            // var result = await _userManager.UpdateAsync(existUser);
            // if (!result.Succeeded)
            // {
            //     throw new InvalidOperationException("Failed to update user.");
            // }

            // Console.WriteLine("Updated user: " + existUser.UserName + " with roles: " + string.Join(", ", newRoleNames));
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

            foreach (var user in users)
            {
                Console.WriteLine("hoho1" + user.UserName + string.Join(", ", user.UserRoles));
                foreach (var userRole in user.UserRoles)
                {
                    Console.WriteLine("haha1" + userRole.RoleId);
                }
            }

            // Retrieve all roles once
            var allRoles = await _roleManager.Roles.ToListAsync();

            // Create a dictionary to map role names to role IDs for quick lookup
            var roleIdLookup = allRoles
                .Where(role => role.Name != null)
                .ToDictionary(role => role.Name!, role => role.Id);

            // Assign roles to each user
            // Assign roles to each user
            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user); // Get roles for the user
                user.UserRoles.Clear(); // Clear the existing UserRoles

                foreach (var roleName in userRoles)
                {
                    if (roleIdLookup.TryGetValue(roleName, out var roleId))
                    {
                        user.AddUserRole(new UserRoles
                        {
                            UserId = user.Id,
                            RoleId = roleId,
                            AssignedAt = DateTime.UtcNow
                        });
                    }
                }

                // Log the user information and roles
                Console.WriteLine($"User: {user.UserName}, Roles: {string.Join(", ", userRoles)}");
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
