using Bingo.BackEnd.Persistance.Entities;
using Bingo.Common.DomainModel.MasterData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bingo.BackEnd.Server.Controllers.Timing
{
    [ApiController]
    [Route("api/[controller]")]
    public class SectorTimeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SectorTimeController> _logger;

        public SectorTimeController(ILogger<SectorTimeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: api/sectortime
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SectorTime>>> GetSectorTimes()
        {
            var sectorTimes = new List<SectorTime>();
            try
            {
                sectorTimes = await _context.SectorTimes.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting sector-times");
                return StatusCode(500, $"Error accessing the database: {ex.Message}");
            }

            return Ok(sectorTimes);
        }

        // GET: api/sectortime/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<SectorTime>> GetSectorTimeById(Guid id)
        {
            var sectorTime = await _context.SectorTimes.FindAsync(id);

            if (sectorTime == null)
            {
                return NotFound("This sector-time does not exist");
            }

            return Ok(sectorTime);
        }

        // POST: api/sectortime
        [HttpPost]
        public async Task<ActionResult<SectorTime>> InsertSectorTime(SectorTime request)
        {
            var newSectorTime = new SectorTime();
            newSectorTime.Id = Guid.NewGuid();
            newSectorTime.CompetitorId = request.CompetitorId;
            newSectorTime.SectorId = request.SectorId;
            if(request.EntryTime != default(DateTime))
            {
                newSectorTime.EntryTime = request.EntryTime;
            }
            if (request.ExitTime != default(DateTime))
            {
                newSectorTime.ExitTime = request.ExitTime;
            }
            newSectorTime.PenaltyTimeTicks = request.PenaltyTimeTicks;
            newSectorTime.PenaltyPositions = request.PenaltyPositions;
            newSectorTime.PenaltyNote = request.PenaltyNote;

            var timeStamp = DateTime.UtcNow;
            newSectorTime.CreationTimeStamp = timeStamp;
            newSectorTime.LastUpdateTimeStamp = timeStamp;
            _context.SectorTimes.Add(newSectorTime);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSectorTimeById), new { id = newSectorTime.Id }, newSectorTime);
        }


        // PUT: api/sectortime
        [HttpPut]
        public async Task<ActionResult<SectorTime>> UpdateSectorTime(SectorTime request)
        {
            var updatedSectorTime = await _context.SectorTimes.FindAsync(request.Id);
            if (updatedSectorTime == null)
            {
                return NotFound();
            }

            updatedSectorTime.CompetitorId = request.CompetitorId;
            updatedSectorTime.SectorId = request.SectorId;
            if (request.EntryTime == default(DateTime))
            {
                updatedSectorTime.EntryTime = DateTime.MinValue;
            }
            else
            {
                updatedSectorTime.EntryTime = request.EntryTime;
            }
            if (request.ExitTime == default(DateTime))
            {
                updatedSectorTime.ExitTime = DateTime.MinValue;
            }
            else
            {
                updatedSectorTime.ExitTime = request.ExitTime;
            }
            updatedSectorTime.PenaltyTimeTicks = request.PenaltyTimeTicks;
            updatedSectorTime.PenaltyPositions = request.PenaltyPositions;
            updatedSectorTime.PenaltyNote = request.PenaltyNote;
            updatedSectorTime.LastUpdateTimeStamp = DateTime.UtcNow;

            _context.Update(updatedSectorTime);
            await _context.SaveChangesAsync();
            return Ok(updatedSectorTime);
        }

        // DELETE: api/sectortime/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSectorTime(Guid id)
        {
            var sectorTimeToDelete = await _context.SectorTimes.FindAsync(id);
            if (sectorTimeToDelete == null)
            {
                return NotFound();
            }

            _context.SectorTimes.Remove(sectorTimeToDelete);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}