using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeProject.Entity;
using RecipeProject.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RecipeProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            var list = _ingredientService.GetAll(includeDeleted);
            return Ok(list);
        }

        // GET: api/Ingredients/5
        [HttpGet("{id}")]
        public IActionResult GetIngredient(int id)
        {
            var ingredient = _ingredientService.GetById(id);

            if (ingredient == null)
            {
                return NotFound();
            }

            return Ok(ingredient);
        }

        // POST: api/Ingredients
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Ingredient>> PostIngredient(Ingredient ingredient)
        {
            return await _ingredientService.AddIngredient(ingredient);
        }

        // PUT: api/Ingredients/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIngredient(int id, Ingredient ingredient)
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

        // DELETE: api/Ingredients/5
        [HttpDelete("{id}")]
        public async void DeleteIngredient(int id)
        {
            await _ingredientService.DeleteIngredient(id);
        }

        private bool IngredientExists(int id)
        {
            return _ingredientService.GetById(id) != null;
        }

        // GET: api/Ingredients/InRecipes/5
        [HttpGet("/InRecipes/{id}")]
        public IActionResult GetAllRecipesContaining(int id)
        {
            var list = _ingredientService.GetAllRecipesContaining(id);
            return Ok(list);
        }
    }
}
