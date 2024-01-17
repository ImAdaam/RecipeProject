using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeProject.Entity;
using RecipeProject.Services;

namespace RecipeProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientCategoryController : ControllerBase
    {
        private readonly IIngredientCategoryService _ingredientCategoryService;

        public IngredientCategoryController(IIngredientCategoryService ingredientCategoryService)
        {
            _ingredientCategoryService = ingredientCategoryService;
        }
        // GET: api/IngredientCategories/{includeDeleted}
        [HttpGet("includeDeleted")]
        public IActionResult List(bool includeDeleted)
        {
            var list = _ingredientCategoryService.GetAll(includeDeleted);
            return Ok(list);
        }

        // GET: api/IngredientCategories/5
        [HttpGet("{id}")]
        public IActionResult GetIngredientCategory(int id)
        {
            var ingredientCategory = _ingredientCategoryService.GetById(id);

            if (ingredientCategory == null)
            {
                return NotFound();
            }

            return Ok(ingredientCategory);
        }

        // POST: api/IngredientCategories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<IngredientCategory>> PostIngredient(IngredientCategory ingredientCategory)
        {
            return await _ingredientCategoryService.AddIngredientCategory(ingredientCategory);
        }

        // PUT: api/IngredientCategories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIngredientCategory(int id, IngredientCategory ingredientCategory)
        {
            if (id != ingredientCategory.Id)
            {
                return BadRequest();
            }

            try
            {
                await _ingredientCategoryService.UpdateIngredientCategory(ingredientCategory);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (IngredientCategoryExists(id))
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

        // DELETE: api/IngredientCategories/5
        [HttpDelete("{id}")]
        public async void DeleteIngredientCategory(int id)
        {
            await _ingredientCategoryService.DeleteIngredientCategory(id);
        }

        private bool IngredientCategoryExists(int id)
        {
            return _ingredientCategoryService.GetById(id) != null;
        }

        // GET: api/IngredientCategories/AllIngredients/{categoryName}
        [HttpGet("/AllIngredients/categoryName")]
        public IActionResult GetAllIngredient(string categoryName)
        {
            var list = _ingredientCategoryService.GetAllIngredient(categoryName);
            return Ok(list);
        }
    }
}
