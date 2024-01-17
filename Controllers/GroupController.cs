using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeProject.Entity;
using RecipeProject.Services;

namespace RecipeProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            var list = _groupService.GetAll(includeDeleted);
            return Ok(list);
        }

        // GET: api/Groups/5
        [HttpGet("{id}")]
        public IActionResult GetGroup(int id)
        {
            var group = _groupService.GetById(id);

            if (group == null)
            {
                return NotFound();
            }

            return Ok(group);
        }


        // PUT: api/Groups/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroup(int id, Group group)
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

        // POST: api/Groups
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Group>> PostGroup(Group group)
        {
            return await _groupService.AddGroup(group);
        }

        // DELETE: api/Groups/5
        [HttpDelete("{id}")]
        public async void DeleteGroup(int id)
        {
            await _groupService.DeleteGroup(id);
        }

        private bool GroupExists(int id)
        {
            return _groupService.GetById(id) != null;
        }
    }
}
