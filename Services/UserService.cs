using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Security.Claims;
using RecipeProject.Entity.DTO;
using RecipeProject.Entity;
using System.Xml.Linq;
using System.Diagnostics.Metrics;

namespace RecipeProject.Services
{
    public interface IUserService
    {
        public Task InitRoles();
        public Task InitUsers();
    }
    public class UserService : IUserService
    {
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(RoleManager<IdentityRole<int>> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task InitRoles()
        {
            await CreateRole(Roles.ADMIN);
            await CreateRole(Roles.RECIPEWRITER);
            await CreateRole(Roles.RECIPEREADER);
        }

        public async Task InitUsers()
        {
            await CreateUser("recipe_admin", "admin", "Veszprém", "MO", "random/picture/idk1.jpg","Admin_1234", Roles.ADMIN);
            await CreateUser("recipe_writer", "writer", "Veszprém", "MO", "random/picture/idk2.jpg", "RecipeReader_1234", Roles.RECIPEWRITER);
            await CreateUser("recipe_reader", "reader", "Veszprém", "MO", "random/picture/idk3.jpg", "RecipeWriter_1234", Roles.RECIPEREADER);
        }

        private async Task CreateUser(string userName, string name,
                                      string city, string country, string profilePicture,
                                      string password, string role)
        {
            ApplicationUser admin = new ApplicationUser()
            {
                UserName = userName,
                EmailConfirmed = true,
                Name = name,
                City = city,
                Country = country,
                ProfilePicture = profilePicture

            };
            var user = await _userManager.CreateAsync(admin, password);
            if (user.Succeeded)
            {
                await _userManager.AddToRoleAsync(admin, role);
                await _userManager.AddClaimAsync(admin, new Claim(ClaimTypes.Role, role));
            }
        }
        private async Task CreateRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                await _roleManager.CreateAsync(new IdentityRole<int>(roleName));
            }
        }
    }
}