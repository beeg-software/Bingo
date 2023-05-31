using Bingo.BackEnd.Persistance.Entities;
using Bingo.Common.DomainModel.MasterData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bingo.BackEnd.Server.Controllers.Setup
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessionSectorController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SessionSectorController> _logger;

        public SessionSectorController(ILogger<SessionSectorController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: api/sessionsector
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SessionSector>>> GetSessionSectors()
        {
            var sessionSectors = new List<SessionSector>();
            try
            {
                sessionSectors = await _context.SessionSectors.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting session-sectors");
                return StatusCode(500, $"Error accessing the database: {ex.Message}");
            }

            return Ok(sessionSectors);
        }

        // GET: api/sessionsector/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<SessionSector>> GetSessionSectorById(Guid id)
        {
            var sessionSector = await _context.SessionSectors.FindAsync(id);

            if (sessionSector == null)
            {
                return NotFound("This session-sector does not exist");
            }

            return Ok(sessionSector);
        }

        // POST: api/sessionsector
        [HttpPost]
        public async Task<ActionResult<SessionSector>> InsertSessionSector(SessionSector request)
        {
            var newSessionSector = new SessionSector();
            newSessionSector.Id = Guid.NewGuid();
            newSessionSector.SessionId = request.SessionId;
            newSessionSector.SectorId = request.SectorId;
            newSessionSector.RaceEnabled = request.RaceEnabled;
            var timeStamp = DateTime.UtcNow;
            newSessionSector.CreationTimeStamp = timeStamp;
            newSessionSector.LastUpdateTimeStamp = timeStamp;
            _context.SessionSectors.Add(newSessionSector);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSessionSectorById), new { id = newSessionSector.Id }, newSessionSector);
        }


        // PUT: api/sessionsector
        [HttpPut]
        public async Task<ActionResult<SessionSector>> UpdateSessionSector(SessionSector request)
        {
            var updatedSessionSector = await _context.SessionSectors.FindAsync(request.Id);
            if (updatedSessionSector == null)
            {
                return NotFound();
            }

            updatedSessionSector.SessionId = request.SessionId;
            updatedSessionSector.SectorId = request.SectorId;
            updatedSessionSector.RaceEnabled = request.RaceEnabled;
            updatedSessionSector.LastUpdateTimeStamp = DateTime.UtcNow;

            _context.Update(updatedSessionSector);
            await _context.SaveChangesAsync();
            return Ok(updatedSessionSector);
        }

        // DELETE: api/sessionsector/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSessionSector(Guid id)
        {
            var sessionSectorToDelete = await _context.SessionSectors.FindAsync(id);
            if (sessionSectorToDelete == null)
            {
                return NotFound();
            }

            _context.SessionSectors.Remove(sessionSectorToDelete);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}