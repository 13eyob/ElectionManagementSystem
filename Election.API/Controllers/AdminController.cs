using Election.DATA;
using Election.DATA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Election.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // ELECTION CONTROL
        // =========================

        // GET: api/admin/election-status
        [HttpGet("election-status")]
        public async Task<IActionResult> GetElectionStatus()
        {
            var settings = await GetOrCreateSettingsAsync();
            return Ok(new
            {
                isActive = settings.IsElectionActive,
                votingOpen = settings.VotingOpen,
                startTime = settings.StartTime,
                endTime = settings.EndTime,
                title = settings.ElectionTitle
            });
        }

        // POST: api/admin/start-election
        [HttpPost("start-election")]
        public async Task<IActionResult> StartElection([FromBody] StartElectionRequest request)
        {
            var settings = await GetOrCreateSettingsAsync();

            settings.IsElectionActive = true;
            settings.VotingOpen = true;
            settings.StartTime = DateTime.UtcNow;
            settings.EndTime = request.EndTime;
            settings.ElectionTitle = request.ElectionTitle ?? "General Election";
            settings.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Election started" });
        }

        // POST: api/admin/stop-election
        [HttpPost("stop-election")]
        public async Task<IActionResult> StopElection()
        {
            var settings = await GetOrCreateSettingsAsync();

            settings.IsElectionActive = false;
            settings.VotingOpen = false;
            settings.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Election stopped" });
        }

        // POST: api/admin/toggle-voting
        [HttpPost("toggle-voting")]
        public async Task<IActionResult> ToggleVoting()
        {
            var settings = await GetOrCreateSettingsAsync();

            if (!settings.IsElectionActive)
                return BadRequest(new { success = false, message = "Election not active" });

            settings.VotingOpen = !settings.VotingOpen;
            settings.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                votingOpen = settings.VotingOpen,
                message = settings.VotingOpen ? "Voting opened" : "Voting closed"
            });
        }

        // =========================
        // CANDIDATE APPROVAL
        // =========================

        // PUT: api/admin/approve-candidate/{id}
        [HttpPut("approve-candidate/{id}")]
        public async Task<IActionResult> ApproveCandidate(int id)
        {
            var candidate = await _context.Candidates.FindAsync(id);
            if (candidate == null)
                return NotFound(new { success = false, message = "Candidate not found" });

            candidate.IsApproved = true;
            candidate.Status = "Approved";

            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Candidate approved" });
        }

        // =========================
        // VOTERS
        // =========================

        // GET: api/admin/voters
        [HttpGet("voters")]
        public async Task<IActionResult> GetAllVoters()
        {
            var voters = await _context.Users
                .Select(u => new
                {
                    u.Id,
                    u.Username,
                    u.Email,
                    u.Role,
                    u.Region,
                    IsCandidate = _context.Candidates.Any(c => c.Email == u.Email),
                    IsActive = true
                })
                .ToListAsync();

            return Ok(new { success = true, voters });
        }

        // =========================
        // REAL ELECTION RESULTS
        // =========================

        // GET: api/admin/election-results
        [HttpGet("election-results")]
        public async Task<IActionResult> GetElectionResults()
        {
            var totalVotes = await _context.Votes.CountAsync();

            var results = await _context.Candidates
                .Where(c => c.IsApproved)
                .Select(c => new
                {
                    c.Id,
                    c.FullName,
                    c.PartyAffiliation,
                    c.Region,
                    Votes = _context.Votes.Count(v => v.CandidateId == c.Id),
                    Percentage = totalVotes == 0
                        ? 0
                        : Math.Round(
                            (_context.Votes.Count(v => v.CandidateId == c.Id) * 100.0) / totalVotes,
                            2)
                })
                .OrderByDescending(r => r.Votes)
                .ToListAsync();

            return Ok(new
            {
                success = true,
                results,
                totalVotes
            });
        }

        // =========================
        // INTERNAL HELPERS
        // =========================

        private async Task<ElectionSettings> GetOrCreateSettingsAsync()
        {
            var settings = await _context.ElectionSettings.FirstOrDefaultAsync();

            if (settings == null)
            {
                settings = new ElectionSettings();
                _context.ElectionSettings.Add(settings);
                await _context.SaveChangesAsync();
            }

            return settings;
        }
    }

    public class StartElectionRequest
    {
        public DateTime? EndTime { get; set; }
        public string? ElectionTitle { get; set; }
    }
}
