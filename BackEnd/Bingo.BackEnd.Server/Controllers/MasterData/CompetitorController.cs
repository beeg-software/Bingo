using Bingo.BackEnd.Persistance.Entities;
using Bingo.Common.DomainModel.MasterData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bingo.BackEnd.Server.Controllers.MasterData
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompetitorController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CompetitorController> _logger;

        public CompetitorController(ILogger<CompetitorController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: api/competitor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Competitor>>> GetCompetitors()
        {
            var competitors = new List<Competitor>();
            try
            {
                competitors = await _context.Competitors.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting competitors");
                return StatusCode(500, $"Error accessing the database: {ex.Message}");
            }

            return Ok(competitors);
        }

        // GET: api/competitor/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Competitor>> GetCompetitorById(Guid id)
        {
            var competitor = await _context.Competitors.FindAsync(id);

            if (competitor == null)
            {
                return NotFound("This competitor does not exist");
            }

            return Ok(competitor);
        }

        // POST: api/competitor
        [HttpPost]
        public async Task<ActionResult<Competitor>> InsertCompetitor(Competitor request)
        {
            var newCompetitor = new Competitor();
            newCompetitor.Id = Guid.NewGuid();
            newCompetitor.Number = request.Number;
            newCompetitor.ImportNumber = request.ImportNumber;
            newCompetitor.CompetitorCategoryId = request.CompetitorCategoryId;
            if (!string.IsNullOrWhiteSpace(request.Name1))
            {
                newCompetitor.Name1 = request.Name1;
            }
            else
            {
                newCompetitor.Name1 = "";
            }
            newCompetitor.Name2 = request.Name2;
            newCompetitor.Name3 = request.Name3;
            newCompetitor.Name4 = request.Name4;
            newCompetitor.Nationality = request.Nationality;
            newCompetitor.Team = request.Team;
            newCompetitor.Boat = request.Boat;
            newCompetitor.Engine = request.Engine;
            newCompetitor.Status = request.Status;
            var timeStamp = DateTime.UtcNow;
            newCompetitor.CreationTimeStamp = timeStamp;
            newCompetitor.LastUpdateTimeStamp = timeStamp;
            _context.Competitors.Add(newCompetitor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCompetitorById), new { id = newCompetitor.Id }, newCompetitor);
        }

        // PUT: api/competitor
        [HttpPut]
        public async Task<ActionResult<Competitor>> UpdateCompetitor(Competitor request)
        {
            var updatedCompetitor = await _context.Competitors.FindAsync(request.Id);
            if (updatedCompetitor == null)
            {
                return NotFound();
            }

            updatedCompetitor.Number = request.Number;
            updatedCompetitor.ImportNumber = request.ImportNumber;
            updatedCompetitor.CompetitorCategoryId = request.CompetitorCategoryId;
            if (!string.IsNullOrWhiteSpace(request.Name1))
            {
                updatedCompetitor.Name1 = request.Name1;
            }
            else
            {
                updatedCompetitor.Name1 = "";
            }
            updatedCompetitor.Name1 = request.Name1;
            updatedCompetitor.Name2 = request.Name2;
            updatedCompetitor.Name3 = request.Name3;
            updatedCompetitor.Name4 = request.Name4;
            updatedCompetitor.Nationality = request.Nationality;
            updatedCompetitor.Team = request.Team;
            updatedCompetitor.Boat = request.Boat;
            updatedCompetitor.Engine = request.Engine;
            updatedCompetitor.Status = request.Status;
            updatedCompetitor.LastUpdateTimeStamp = DateTime.UtcNow;

            _context.Update(updatedCompetitor);
            await _context.SaveChangesAsync();
            return Ok(updatedCompetitor);
        }

        // DELETE: api/competitor/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompetitor(Guid id)
        {
            var competitorToDelete = await _context.Competitors.FindAsync(id);
            if (competitorToDelete == null)
            {
                return NotFound();
            }

            _context.Competitors.Remove(competitorToDelete);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}