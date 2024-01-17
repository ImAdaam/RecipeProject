using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeProject.Entity;
using RecipeProject.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RecipeProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AllergenController : ControllerBase
    {
        private readonly IAllergenService _allergenService;

        public AllergenController(IAllergenService allergenService)
        {
            _allergenService = allergenService;
        }
        // GET: api/Allergens/{includeDeleted}
        [HttpGet("includeDeleted")]
        public IActionResult List(bool includeDeleted)
        {
            var list = _allergenService.GetAll(includeDeleted);
            return Ok(list);
        }

        // GET: api/Allergens/5
        [HttpGet("{id}")]
        public IActionResult GetAllergen(int id)
        {
            var allergen = _allergenService.GetById(id);

            if (allergen == null)
            {
                return NotFound();
            }

            return Ok(allergen);
        }


        // PUT: api/Allergens/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAllergen(int id, Allergen allergen)
        {
            if (id != allergen.Id)
            {
                return BadRequest();
            }

            try
            {
                await _allergenService.UpdateAllergen(allergen);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (AllergenExists(id))
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

        // POST: api/Allergens
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Allergen>> PostAllergen(Allergen allergen)
        {
            return await _allergenService.AddAllergen(allergen);
        }

        // DELETE: api/Allergens/5
        [HttpDelete("{id}")]
        public async void DeleteAllergen(int id)
        {
            await _allergenService.DeleteAllergen(id);
        }

        private bool AllergenExists(int id)
        {
            return _allergenService.GetById(id) != null;
        }
    }
}
