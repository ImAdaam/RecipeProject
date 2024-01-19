using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeProject.Entity;
using RecipeProject.Exceptions;
using RecipeProject.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RecipeProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class IngredientController : ControllerBase
    {
        private readonly IIngredientService _ingredientService;

        public IngredientController(IIngredientService ingredientService)
        {
            _ingredientService = ingredientService;
        }
        // GET: api/Ingredients/{includeDeleted}
        [HttpGet("includeDeleted")]
        public IActionResult List(bool includeDeleted)
        {
            try
            {
                var list = _ingredientService.GetAll(includeDeleted);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }

        // GET: api/Ingredients/5
        [HttpGet("{id}")]
        public IActionResult GetIngredient(int id)
        {
            try
            {
                var ingredient = _ingredientService.GetById(id);

                if (ingredient == null)
                {
                    return NotFound();
                }

                return Ok(ingredient);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }

        // POST: api/Ingredients
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin,Recipe writer")]
        public async Task<ActionResult<Ingredient>> PostIngredient(Ingredient ingredient)
        {
            try
            {
                return await _ingredientService.AddIngredient(ingredient);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }

        // PUT: api/Ingredients/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Recipe writer")]
        public async Task<IActionResult> PutIngredient(int id, Ingredient ingredient)
        {
            try
            {
                if (id != ingredient.Id)
                {
                    return BadRequest();
                }

                try
                {
                    await _ingredientService.UpdateIngredient(ingredient);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (IngredientExists(id))
                    {
                        return StatusCode(500, new { Message = "Internal Server Error: record already in db" });
                    }
                    else
                    {
                        return StatusCode(500, new { Message = "Internal Server Error: sql server error" });
                    }
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }

        // DELETE: api/Ingredients/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteIngredient(int id)
        {
            try
            {
                _ingredientService.DeleteIngredient(id);
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

        private bool IngredientExists(int id)
        {
            return _ingredientService.GetById(id) != null;
        }

        // GET: api/Ingredients/InRecipes/5
        [HttpGet("/InRecipes/{id}")]
        [Authorize(Roles = "Recipe writer,Recipe reader")]
        [Authorize(Policy = "RequireActiveUser")]
        public IActionResult GetAllRecipesContaining(int id)
        {
            try
            {
                var list = _ingredientService.GetAllRecipesContaining(id);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }
    }
}
