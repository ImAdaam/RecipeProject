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
    public class IngredientAllergensController : ControllerBase
    {
        //private readonly RecipeDbContext _context;
        private readonly IIngredientAllergenService _ingredientAllergenService;

        public IngredientAllergensController(IIngredientAllergenService ingredientAllergenService)
        {
            _ingredientAllergenService = ingredientAllergenService;
        }

        // GET: api/IngredientAllergens
        [HttpGet]
        public ActionResult<IEnumerable<IngredientAllergen>> GetIngredientAllergens()
        {
            try
            {
                var list = _ingredientAllergenService.GetAll();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }

        // GET: api/IngredientAllergens/5
        [HttpGet("{id}")]
        public ActionResult<IngredientAllergen> GetIngredientAllergen(int ingredientId, int allergenId)
        {
            try
            {
                var ingredientAllergen = _ingredientAllergenService.GetById(ingredientId, allergenId);

                if (ingredientAllergen == null)
                {
                    return NotFound();
                }

                return Ok(ingredientAllergen);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }

        // PUT: api/IngredientAllergens/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public Task<IActionResult> PutIngredientAllergen(int id, IngredientAllergen ingredientAllergen)
        {
            throw new NotImplementedException();
        }

        // POST: api/IngredientAllergens
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<IngredientAllergen>> PostIngredientAllergen(IngredientAllergen ingredientAllergen)
        {
            try
            {
                return await _ingredientAllergenService.AddIngredientAllergen(ingredientAllergen);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }

        // DELETE: api/IngredientAllergens/5
        [HttpDelete("{id}")]
        public IActionResult DeleteIngredientAllergen(int ingredientId, int allergenId)
        {
            try
            {
                _ingredientAllergenService.DeleteIngredientAllergen(ingredientId, allergenId);
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

        private bool IngredientAllergenExists(int ingredientId, int allergenId)
        {
            return _ingredientAllergenService.GetById(ingredientId, allergenId) != null;
        }
    }
}
