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
            try
            {
                var list = _allergenService.GetAll(includeDeleted);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }

        // GET: api/Allergens/5
        [HttpGet("{id}")]
        public IActionResult GetAllergen(int id)
        {
            try
            {
                var allergen = _allergenService.GetById(id);

                if (allergen == null)
                {
                    return NotFound();
                }

                return Ok(allergen);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }


        // PUT: api/Allergens/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAllergen(int id, Allergen allergen)
        {
            try
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
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }

        // POST: api/Allergens
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Allergen>> PostAllergen(Allergen allergen)
        {
            try { 
                return await _allergenService.AddAllergen(allergen);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message});
            }
        }

        // DELETE: api/Allergens/5
        [HttpDelete("{id}")]
        public IActionResult DeleteAllergen(int id)
        {
            try
            {
                _allergenService.DeleteAllergen(id);
                return Ok();
            }
            catch (MethodNotAllowedException e)
            {
                return StatusCode(405, e.Message);
            }
        }

        private bool AllergenExists(int id)
        {
            return _allergenService.GetById(id) != null;
        }
    }
}
