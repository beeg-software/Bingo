using Bingo.BackEnd.Persistance.Entities;
using Bingo.Common.DomainModel.Setup;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bingo.BackEnd.Server.Controllers.Setup
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SessionController> _logger;

        public SessionController(ILogger<SessionController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: api/session
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Session>>> GetSessions()
        {
            var sessions = new List<Session>();
            try
            {
                sessions = await _context.Sessions.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting sessions");
                return StatusCode(500, $"Error accessing the database: {ex.Message}");
            }

            return Ok(sessions);
        }

        // GET: api/session/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Session>> GetSessionById(Guid id)
        {
            var session = await _context.Sessions.FindAsync(id);

            if (session == null)
            {
                return NotFound("This session does not exist");
            }

            return Ok(session);
        }

        // POST: api/session
        [HttpPost]
        public async Task<ActionResult<Session>> InsertSession(Session request)
        {
            var newSession = new Session();
            newSession.Id = Guid.NewGuid();
            newSession.Name = request.Name;
            var timeStamp = DateTime.UtcNow;
            newSession.CreationTimeStamp = timeStamp;
            newSession.LastUpdateTimeStamp = timeStamp;
            _context.Sessions.Add(newSession);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSessionById), new { id = newSession.Id }, newSession);
        }


        // PUT: api/session
        [HttpPut]
        public async Task<ActionResult<Session>> UpdateSession(Session request)
        {
            var updatedSession = await _context.Sessions.FindAsync(request.Id);
            if (updatedSession == null)
            {
                return NotFound();
            }

            updatedSession.Name = request.Name;
            updatedSession.LastUpdateTimeStamp = DateTime.UtcNow;

            _context.Update(updatedSession);
            await _context.SaveChangesAsync();
            return Ok(updatedSession);
        }

        // DELETE: api/session/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSession(Guid id)
        {
            var sessionToDelete = await _context.Sessions.FindAsync(id);
            if (sessionToDelete == null)
            {
                return NotFound();
            }

            _context.Sessions.Remove(sessionToDelete);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}