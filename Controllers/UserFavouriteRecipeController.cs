using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeProject.Entity;
using RecipeProject.Services;
using RecipeProject.DbContext;
using RecipeProject.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace RecipeProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserFavouriteRecipeController : ControllerBase
    {
        private readonly IUserFavouriteRecipeService _userFavouriteRecipeService;

        public UserFavouriteRecipeController(IUserFavouriteRecipeService userFavouriteRecipeService)
        {
            _userFavouriteRecipeService = userFavouriteRecipeService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserFavouriteRecipe>> GetUserFavouriteRecipes()
        {
            try
            {
                var list = _userFavouriteRecipeService.GetAll();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }

        [HttpGet("{id}")]
        public ActionResult<UserFavouriteRecipe> GetUserFavouriteRecipe(int recipeId, int userId)
        {
            try
            {
                var userFavouriteRecipe = _userFavouriteRecipeService.GetById(recipeId, userId);

                if (userFavouriteRecipe == null)
                {
                    return NotFound();
                }

                return Ok(userFavouriteRecipe);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }

        [HttpPut("{id}")]
        public Task<IActionResult> PutUserFavouriteRecipe(int id, UserFavouriteRecipe userFavouriteRecipe)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<ActionResult<UserFavouriteRecipe>> PostUserFavouriteRecipe(UserFavouriteRecipe userFavouriteRecipe)
        {
            try
            {
                return await _userFavouriteRecipeService.AddUserFavouriteRecipe(userFavouriteRecipe);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUserFavouriteRecipe(int recipeId, int userId)
        {
            try
            {
                _userFavouriteRecipeService.DeleteUserFavouriteRecipe(recipeId, userId);
                return Ok();
            }
            catch (MethodNotAllowedException e)
            {
                return StatusCode(405, e.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }

        private bool UserFavouriteRecipeExists(int recipeId, int userId)
        {
            return _userFavouriteRecipeService.GetById(recipeId, userId) != null;
        }
    }
}
