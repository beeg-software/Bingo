using Blank7.BackEnd.Persistance.Entities;
using Blank7.Common.DomainModel.MasterData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace Blank7.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: api/user
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = new List<User>();
            try
            {
                users = await _context.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting users");
                return StatusCode(500, $"Error accessing the database: {ex.Message}");
            }

            return Ok(users);
        }

        // GET: api/user/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound("This user does not exist");
            }

            return Ok(user);
        }

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult<User>> InsertUser(User request)
        {
            var newUser = new User(Guid.NewGuid(), request.Name, DateTime.UtcNow);
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
        }


        // PUT: api/user/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, User request)
        {
            var updatedUser = await _context.Users.FindAsync(id);
            if (updatedUser == null)
            {
                return NotFound();
            }

            updatedUser.Name = request.Name;
            updatedUser.TimeStamp = DateTime.UtcNow;

            _context.Update(updatedUser);
            await _context.SaveChangesAsync();
            return Ok(updatedUser);
        }

        // DELETE: api/user/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var userToDelete = await _context.Users.FindAsync(id);
            if (userToDelete == null)
            {
                return NotFound();
            }

            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}