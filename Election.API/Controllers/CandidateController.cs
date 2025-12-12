using Election.DATA;
using Election.DATA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Election.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _environment;

        public CandidateController(AppDbContext db, IWebHostEnvironment environment)
        {
            _db = db;
            _environment = environment;
        }

        // KEEP YOUR EXISTING SubmitCandidate METHOD AS IS

        // GET candidate by email
        [HttpGet("byemail/{email}")]
        public async Task<IActionResult> GetCandidateByEmail(string email)
        {
            try
            {
                var candidate = await _db.Candidates
                    .FirstOrDefaultAsync(c => c.Email == email);

                if (candidate == null)
                    return NotFound(new { success = false, message = "Candidate not found" });

                return Ok(new
                {
                    success = true,
                    Id = candidate.Id,
                    FullName = candidate.FullName,
                    Age = candidate.Age,
                    Region = candidate.Region,
                    PartyAffiliation = candidate.PartyAffiliation,
                    Email = candidate.Email,
                    Phone = candidate.Phone,
                    ApplicationDate = candidate.ApplicationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    Status = candidate.Status
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // UPDATE candidate
        [HttpPut("update")]
        public async Task<IActionResult> UpdateCandidate([FromBody] UpdateCandidateRequest model)
        {
            try
            {
                var candidate = await _db.Candidates.FindAsync(model.Id);
                if (candidate == null)
                    return NotFound(new { success = false, message = "Candidate not found" });

                // Update fields
                candidate.FullName = model.FullName;
                candidate.Age = model.Age;
                candidate.Region = model.Region;
                candidate.PartyAffiliation = model.PartyAffiliation;
                candidate.Phone = model.Phone;
                //candidate.UpdatedAt = DateTime.Now;

                await _db.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = "Candidate profile updated successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        // DELETE candidate
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCandidate(int id)
        {
            try
            {
                var candidate = await _db.Candidates.FindAsync(id);
                if (candidate == null)
                    return NotFound(new { success = false, message = "Candidate not found" });

                _db.Candidates.Remove(candidate);
                await _db.SaveChangesAsync();

                return Ok(new { success = true, message = "Candidate application deleted" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Error: {ex.Message}" });
            }
        }
    }

    // ADD THIS CLASS for update request
    public class UpdateCandidateRequest
    {
        public int Id { get; set; }
        public string FullName { get; set; } = "";
        public int Age { get; set; }
        public string Region { get; set; } = "";
        public string PartyAffiliation { get; set; } = "";
        public string Phone { get; set; } = "";
    }
}