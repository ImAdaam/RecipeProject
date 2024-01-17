using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeProject.Entity;
using RecipeProject.Services;

namespace RecipeProject.Controllers
{
    public class RecipeController
    {
        [Route("api/[controller]")]
        [ApiController]
        public class RecipesController : ControllerBase
        {
            private readonly IRecipeService _recipeService;

            public RecipesController(IRecipeService recipeService)
            {
                _recipeService = recipeService;
            }

            // GET: api/Recipes/{includeDeleted}
            [HttpGet("includeDeleted")]
            public IActionResult List(bool includeDeleted)
            {
                var list = _recipeService.GetAll(includeDeleted);
                return Ok(list);
            }

            // GET: api/Recipes/5
            [HttpGet("{id}")]
            public IActionResult GetRecipe(int id)
            { 
                var recipe = _recipeService.GetById(id);

                if (recipe == null)
                {
                    return NotFound();
                }

                return Ok(recipe);
            }

            // GET: api/Recipes/5
            [HttpGet("/full/{id}")]
            public IActionResult GetFullRecipe(int id)
            {
                var recipe = _recipeService.GetByIdFull(id);

                if (recipe == null)
                {
                    return NotFound();
                }

                return Ok(recipe);
            }

            // PUT: api/Recipes/5
            // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
            [HttpPut("{id}")]
            public async Task<IActionResult> PutRecipe(int id, Recipe recipe)
            {
                if (id != recipe.Id)
                {
                    return BadRequest();
                }

                try
                {
                    await _recipeService.UpdateRecipe(recipe);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (RecipeExists(id))
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

            // POST: api/Recipes
            // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
            [HttpPost]
            public async Task<ActionResult<Recipe>> PostRecipe(Recipe recipe)
            {
                return await _recipeService.AddRecipe(recipe);
            }

            // DELETE: api/Recipes/5
            [HttpDelete("{id}")]
            public async void DeleteRecipe(int id)
            {
                await _recipeService.DeleteRecipe(id);
            }

            private bool RecipeExists(int id)
            {
                return _recipeService.GetById(id) != null;
            }
        }
    }
}
