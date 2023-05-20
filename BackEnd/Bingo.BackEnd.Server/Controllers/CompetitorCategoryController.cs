using Bingo.BackEnd.Persistance.Entities;
using Bingo.Common.DomainModel.MasterData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bingo.BackEnd.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompetitorCategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CompetitorCategoryController> _logger;

        public CompetitorCategoryController(ILogger<CompetitorCategoryController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: api/competitorcategory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompetitorCategory>>> GetCompetitorCategories()
        {
            var competitorCategories = new List<CompetitorCategory>();
            try
            {
                competitorCategories = await _context.CompetitorCategories.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting competitor categories");
                return StatusCode(500, $"Error accessing the database: {ex.Message}");
            }

            return Ok(competitorCategories);
        }

        // GET: api/competitorcategory/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CompetitorCategory>> GetCompetitorCategoryById(Guid id)
        {
            var competitorCategory = await _context.CompetitorCategories.FindAsync(id);

            if (competitorCategory == null)
            {
                return NotFound("This competitor category does not exist");
            }

            return Ok(competitorCategory);
        }

        // POST: api/competitorcategory
        [HttpPost]
        public async Task<ActionResult<CompetitorCategory>> InsertCompetitorCategory(CompetitorCategory request)
        {
            var newCompetitorCategory = new CompetitorCategory();
            newCompetitorCategory.Id = Guid.NewGuid();
            newCompetitorCategory.Name = request.Name;
            var timeStamp = DateTime.UtcNow;
            newCompetitorCategory.CreationTimeStamp = timeStamp;
            newCompetitorCategory.LastUpdateTimeStamp = timeStamp;
            _context.CompetitorCategories.Add(newCompetitorCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCompetitorCategoryById), new { id = newCompetitorCategory.Id }, newCompetitorCategory);
        }

        // PUT: api/competitorcategory
        [HttpPut]
        public async Task<ActionResult<CompetitorCategory>> UpdateCompetitorCategory(CompetitorCategory request)
        {
            var updatedCompetitorCategory = await _context.CompetitorCategories.FindAsync(request.Id);
            if (updatedCompetitorCategory == null)
            {
                return NotFound();
            }

            updatedCompetitorCategory.Name = request.Name;
            updatedCompetitorCategory.LastUpdateTimeStamp = DateTime.UtcNow;

            _context.Update(updatedCompetitorCategory);
            await _context.SaveChangesAsync();
            return Ok(updatedCompetitorCategory);
        }

        // DELETE: api/competitorcategory/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompetitorCategory(Guid id)
        {
            var competitorCategoryToDelete = await _context.CompetitorCategories.FindAsync(id);
            if (competitorCategoryToDelete == null)
            {
                return NotFound();
            }

            _context.CompetitorCategories.Remove(competitorCategoryToDelete);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
