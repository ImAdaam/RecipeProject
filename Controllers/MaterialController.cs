using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeProject.Entity;
using RecipeProject.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RecipeProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MaterialController : ControllerBase
    {
        private readonly IMaterialService _materialService;

        public MaterialController(IMaterialService materialService)
        {
            _materialService = materialService;
        }
        // GET: api/Materials/{includeDeleted}
        [HttpGet("includeDeleted")]
        public IActionResult List(bool includeDeleted)
        {
            try
            {
                var list = _materialService.GetAll(includeDeleted);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }

        // GET: api/Materials/5
        [HttpGet("{id}")]
        public IActionResult GetMaterial(int id)
        {
            try
            {
                var material = _materialService.GetById(id);

                if (material == null)
                {
                    return NotFound();
                }

                return Ok(material);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }


        // PUT: api/Materials/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Recipe writer")]
        public async Task<IActionResult> PutMaterial(int id, Material material)
        {
            try
            {
                if (id != material.Id)
                {
                    return BadRequest();
                }

                try
                {
                    await _materialService.UpdateMaterial(material);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (MaterialExists(id))
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

        // POST: api/Materials
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin,Recipe writer")]
        public async Task<ActionResult<Material>> PostMaterial(Material material)
        {
            try
            {
                return await _materialService.AddMaterial(material);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }

        // DELETE: api/Materials/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteMaterial(int id)
        {
            try
            {
                _materialService.DeleteMaterial(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }

        private bool MaterialExists(int id)
        {
            return _materialService.GetById(id) != null;
        }
    }
}
