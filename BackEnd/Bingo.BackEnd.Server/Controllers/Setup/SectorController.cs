using Bingo.BackEnd.Persistance.Entities;
using Bingo.Common.DomainModel.Setup;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bingo.BackEnd.Server.Controllers.Setup
{
    [ApiController]
    [Route("api/[controller]")]
    public class SectorController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SectorController> _logger;

        public SectorController(ILogger<SectorController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: api/sector
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sector>>> GetSectors()
        {
            var sectors = new List<Sector>();
            try
            {
                sectors = await _context.Sectors.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting sectors");
                return StatusCode(500, $"Error accessing the database: {ex.Message}");
            }

            return Ok(sectors);
        }

        // GET: api/sector/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Sector>> GetSectorById(Guid id)
        {
            var sector = await _context.Sectors.FindAsync(id);

            if (sector == null)
            {
                return NotFound("This sector does not exist");
            }

            return Ok(sector);
        }

        // POST: api/sector
        [HttpPost]
        public async Task<ActionResult<Sector>> InsertSector(Sector request)
        {
            var newSector = new Sector();
            newSector.Id = Guid.NewGuid();
            newSector.Name = request.Name;
            newSector.ImportName = request.ImportName;
            newSector.Length = request.Length;
            newSector.TargetAverageSpeed = request.TargetAverageSpeed;
            newSector.MinTimeTicks = request.MinTimeTicks;
            newSector.MaxTimeTicks = request.MaxTimeTicks;
            var timeStamp = DateTime.UtcNow;
            newSector.CreationTimeStamp = timeStamp;
            newSector.LastUpdateTimeStamp = timeStamp;
            _context.Sectors.Add(newSector);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSectorById), new { id = newSector.Id }, newSector);
        }


        // PUT: api/sector
        [HttpPut]
        public async Task<ActionResult<Sector>> UpdateSector(Sector request)
        {
            var updatedSector = await _context.Sectors.FindAsync(request.Id);
            if (updatedSector == null)
            {
                return NotFound();
            }

            updatedSector.Name = request.Name;
            updatedSector.ImportName = request.ImportName;
            updatedSector.Length = request.Length;
            updatedSector.TargetAverageSpeed = request.TargetAverageSpeed;
            updatedSector.MinTimeTicks = request.MinTimeTicks;
            updatedSector.MaxTimeTicks = request.MaxTimeTicks;
            updatedSector.LastUpdateTimeStamp = DateTime.UtcNow;

            _context.Update(updatedSector);
            await _context.SaveChangesAsync();
            return Ok(updatedSector);
        }

        // DELETE: api/sector/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSector(Guid id)
        {
            var sectorToDelete = await _context.Sectors.FindAsync(id);
            if (sectorToDelete == null)
            {
                return NotFound();
            }

            _context.Sectors.Remove(sectorToDelete);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}