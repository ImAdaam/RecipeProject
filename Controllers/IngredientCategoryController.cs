using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeProject.Entity;
using RecipeProject.Exceptions;
using RecipeProject.Services;

namespace RecipeProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
            try
            {
                var list = _ingredientCategoryService.GetAll(includeDeleted);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }

        // GET: api/IngredientCategories/5
        [HttpGet("{id}")]
        public IActionResult GetIngredientCategory(int id)
        {
            try
            {
                var ingredientCategory = _ingredientCategoryService.GetById(id);

                if (ingredientCategory == null)
                {
                    return NotFound();
                }

                return Ok(ingredientCategory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }

        // POST: api/IngredientCategories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IngredientCategory>> PostIngredient(IngredientCategory ingredientCategory)
        {
            try
            {
                return await _ingredientCategoryService.AddIngredientCategory(ingredientCategory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }

        // PUT: api/IngredientCategories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutIngredientCategory(int id, IngredientCategory ingredientCategory)
        {
            try
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
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }

        // DELETE: api/IngredientCategories/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteIngredientCategory(int id)
        {
            try
            {
                _ingredientCategoryService.DeleteIngredientCategory(id);
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

        private bool IngredientCategoryExists(int id)
        {
            return _ingredientCategoryService.GetById(id) != null;
        }

        // GET: api/IngredientCategories/AllIngredients/{categoryName}
        [HttpGet("/AllIngredients/categoryName")]
        public IActionResult GetAllIngredient(string categoryName)
        {
            try
            {
                var list = _ingredientCategoryService.GetAllIngredient(categoryName);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }
    }
}
