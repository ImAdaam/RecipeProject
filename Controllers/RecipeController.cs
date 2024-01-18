using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeProject.Entity;
using RecipeProject.Services;
using RecipeProject.Exceptions;

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
                try
                {
                    var list = _recipeService.GetAll(includeDeleted);
                    return Ok(list);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { ex.Message});
                }
            }

            // GET: api/Recipes/5
            [HttpGet("{id}")]
            public IActionResult GetRecipe(int id)
            { 
                try
                {
                    var recipe = _recipeService.GetById(id);

                    if (recipe == null)
                    {
                        return NotFound();
                    }

                    return Ok(recipe);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { ex.Message });
                }
            }

            // GET: api/Recipes/5
            [HttpGet("/full/{id}")]
            public IActionResult GetFullRecipe(int id)
            {
                try
                {
                    var recipe = _recipeService.GetByIdFull(id);

                    if (recipe == null)
                    {
                        return NotFound();
                    }

                    return Ok(recipe);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { ex.Message });
                }
            }

            // PUT: api/Recipes/5
            // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
            [HttpPut("{id}")]
            public async Task<IActionResult> PutRecipe(int id, Recipe recipe)
            {
                try
                {
                    if(id != recipe.Id)
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
                catch (Exception ex)
                {
                    return StatusCode(500, new { ex.Message });
                }
            }

            // POST: api/Recipes
            // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
            [HttpPost]
            public async Task<ActionResult<Recipe>> PostRecipe(Recipe recipe)
            {
                try
                {
                    return await _recipeService.AddRecipe(recipe);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { ex.Message });
                }
            }

            // DELETE: api/Recipes/5
            [HttpDelete("{id}")]
            public IActionResult DeleteRecipe(int id)
            {
                try
                {
                    _recipeService.DeleteRecipe(id);
                    return Ok();
                }
                catch (MethodNotAllowedException e){ 
                    return StatusCode(405, e.Message);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { ex.Message });
                }
            }

            private bool RecipeExists(int id)
            {
                return _recipeService.GetById(id) != null;
            }
        }
    }
}
