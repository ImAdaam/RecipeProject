using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeProject.Entity;
using RecipeProject.Services;
using RecipeProject.DbContext;

namespace RecipeProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientAllergensController : ControllerBase
    {
        private readonly RecipeDbContext _context;

        public IngredientAllergensController(RecipeDbContext context)
        {
            _context = context;
        }

        // GET: api/IngredientAllergens
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IngredientAllergen>>> GetIngredientAllergens()
        {
            if (_context.ingredientAllergens == null)
            {
                return NotFound();
            }
            return await _context.ingredientAllergens.ToListAsync();
        }

        // GET: api/IngredientAllergens/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IngredientAllergen>> GetIngredientAllergen(int id)
        {
            if (_context.ingredientAllergens == null)
            {
                return NotFound();
            }
            var ingredientAllergen = await _context.ingredientAllergens.FindAsync(id);

            if (ingredientAllergen == null)
            {
                return NotFound();
            }

            return ingredientAllergen;
        }

        // PUT: api/IngredientAllergens/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIngredientAllergen(int id, IngredientAllergen ingredientAllergen)
        {
            if (id != ingredientAllergen.AllergenId)
            {
                return BadRequest();
            }

            _context.Entry(ingredientAllergen).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IngredientAllergenExists(id))
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

        // POST: api/IngredientAllergens
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<IngredientAllergen>> PostIngredientAllergen(IngredientAllergen ingredientAllergen)
        {
            if (_context.ingredientAllergens == null)
            {
                return Problem("Entity set 'RecipeDbContext.IngredientAllergens'  is null.");
            }
            _context.ingredientAllergens.Add(ingredientAllergen);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (IngredientAllergenExists(ingredientAllergen.AllergenId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetIngredientAllergen", new { id = ingredientAllergen.AllergenId }, ingredientAllergen);
        }

        // DELETE: api/IngredientAllergens/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIngredientAllergen(int id)
        {
            if (_context.ingredientAllergens == null)
            {
                return NotFound();
            }
            var ingredientAllergen = await _context.ingredientAllergens.FindAsync(id);
            if (ingredientAllergen == null)
            {
                return NotFound();
            }

            _context.ingredientAllergens.Remove(ingredientAllergen);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IngredientAllergenExists(int id)
        {
            return (_context.ingredientAllergens?.Any(e => e.AllergenId == id)).GetValueOrDefault();
        }
    }
}
