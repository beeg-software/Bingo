using Blank7.Common.DomainModel.MasterData;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace Blank7.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private List<User> Users = new List<User>();

        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;

            Users.Add(new User(Guid.NewGuid(), "User1", DateTime.UtcNow));
            Users.Add(new User(Guid.NewGuid(), "User2", DateTime.UtcNow));
            Users.Add(new User(Guid.NewGuid(), "User3", DateTime.UtcNow));
        }

        // GET: api/user
        [HttpGet]
        public ActionResult<List<User>> GetUsers()
        {
            return Ok(Users);
        }

        // GET: api/user/{id}
        [HttpGet("{Id}")]
        public ActionResult<User> GetUserById(string Id)
        {
            Guid _id = new Guid(Id);

            User user = null;

            user = Users.FirstOrDefault(us => us.Id == _id);

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
            User newUser = new User(Guid.NewGuid(), request.Name, DateTime.UtcNow);
            Users.Add(newUser);

            return Ok(newUser);
        }

        // PUT: api/user/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> UpdateUser(Guid id, [FromBody] User request)
        {
            User updatedUser = Users.FirstOrDefault(us => us.Id == id);
            if (updatedUser == null)
            {
                return NotFound();
            }

            updatedUser.Name = request.Name;
            updatedUser.TimeStamp = DateTime.UtcNow;
            return Ok(updatedUser);
        }

        // DELETE: api/user/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(Guid id)
        {
            var userToDelete = Users.FirstOrDefault(u => u.Id == id);
            if (userToDelete == null)
            {
                return NotFound();
            }

            Users.Remove(userToDelete);
            return NoContent();
        }
    }
}