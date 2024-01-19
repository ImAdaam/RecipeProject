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
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }
        // GET: api/Groups/{includeDeleted}
        [HttpGet("includeDeleted")]
        public IActionResult List(bool includeDeleted)
        {
            try
            {
                var list = _groupService.GetAll(includeDeleted);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }

        // GET: api/Groups/5
        [HttpGet("{id}")]
        public IActionResult GetGroup(int id)
        {
            try
            {
                var group = _groupService.GetById(id);

                if (group == null)
                {
                    return NotFound();
                }

                return Ok(group);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }


        // PUT: api/Groups/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutGroup(int id, Group group)
        {
            try
            {
                if (id != group.Id)
                {
                    return BadRequest();
                }

                try
                {
                    await _groupService.UpdateGroup(group);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (GroupExists(id))
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

        // POST: api/Groups
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Group>> PostGroup(Group group)
        {
            try
            {
                return await _groupService.AddGroup(group);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }

        // DELETE: api/Groups/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteGroup(int id)
        {
            try
            {
                _groupService.DeleteGroup(id);
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

        private bool GroupExists(int id)
        {
            return _groupService.GetById(id) != null;
        }
    }
}
