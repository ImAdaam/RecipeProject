using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RecipeProject.Entity;
using RecipeProject.Entity.DTO;
using RecipeProject.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RecipeProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public AuthenticationController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _userService = userService;
        }

        /// <summary>
        /// Register user by the provided registration data and return the status based on, whether the process was successful or not
        /// </summary>
        /// <param name="userForRegistration"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        [AllowAnonymous]
        [HttpPost]
        [Route("api/auth/register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDTO userForRegistration)
        {
            if (_userManager.Users.Any(u => u.UserName == userForRegistration.Username || u.Name == userForRegistration.Name))
            {
                throw new ApplicationException("Username/Name already exists!");
            }
            var user = new ApplicationUser
            {
                UserName = userForRegistration.Username,
                Name = userForRegistration.Name,
                City = userForRegistration.City,
                Country = userForRegistration.Country,
                ProfilePicture = userForRegistration.ProfilePicture,
                IsRecipeWriter = false,
            };
            var result = await _userManager.CreateAsync(user, userForRegistration.Password);
            return result.Succeeded ? StatusCode(201) : throw new ApplicationException("Registration failed!");
        }

        /// <summary>
        /// Login the user by username and password and return a generated JWT token
        /// </summary>
        /// <param name="userLoginDTO"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("api/auth/login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginDTO)
        {
            var user = await _userManager.FindByNameAsync(userLoginDTO.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, userLoginDTO.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Actor, user.IsRecipeWriter.ToString())
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = GetToken(authClaims);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(4),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        [HttpPost]
        [Route("api/auth/logout")]
        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/auth/initRoles")]
        public async Task<IActionResult> InitRoles()
        {
            await _userService.InitRoles();
            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/auth/initUsers")]
        public async Task<IActionResult> InitUsers()
        {
            await _userService.InitUsers();
            return Ok();
        }
    }
}
