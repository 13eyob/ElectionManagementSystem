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
        // ✅ EXISTING ELECTION CONTROL (UNCHANGED)
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
        // ✅ EXISTING CANDIDATE APPROVAL (UNCHANGED)
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
        // ✅ EXISTING VOTERS ENDPOINT (UNCHANGED)
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
        // ✅ EXISTING ELECTION RESULTS (UNCHANGED)
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
        // ✅ NEW ENDPOINTS: ADDED FOR ADMIN DASHBOARD
        // =========================

        // ✅ NEW: Get detailed voters list for admin dashboard
        // GET: api/admin/voters-detailed
        [HttpGet("voters-detailed")]
        public async Task<IActionResult> GetVotersDetailed()
        {
            try
            {
                var voters = await _context.Users
                    .Where(u => u.Role == "Voter")
                    .OrderByDescending(u => u.CreatedAt)
                    .Select(u => new
                    {
                        u.Id,
                        u.Username,
                        u.FullName,
                        u.Email,
                        u.Region,
                        u.Age,
                        u.Role,
                        u.IsApproved,
                        u.HasVoted,
                        RegisteredDate = u.CreatedAt,
                        IsCandidate = _context.Candidates.Any(c => c.Email == u.Email)
                    })
                    .ToListAsync();

                return Ok(new { success = true, voters });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // ✅ NEW: Get election statistics for dashboard
        // GET: api/admin/dashboard-statistics
        [HttpGet("dashboard-statistics")]
        public async Task<IActionResult> GetDashboardStatistics()
        {
            try
            {
                var totalVoters = await _context.Users.CountAsync(u => u.Role == "Voter");
                var votedCount = await _context.Users.CountAsync(u => u.Role == "Voter" && u.HasVoted);
                var candidatesCount = await _context.Candidates.CountAsync();
                var approvedCandidates = await _context.Candidates.CountAsync(c => c.IsApproved);
                var pendingCandidates = await _context.Candidates.CountAsync(c => c.Status == "Pending");
                var totalVotes = await _context.Votes.CountAsync();

                return Ok(new
                {
                    success = true,
                    statistics = new
                    {
                        TotalVoters = totalVoters,
                        VotedCount = votedCount,
                        PendingVoters = totalVoters - votedCount,
                        TotalCandidates = candidatesCount,
                        ApprovedCandidates = approvedCandidates,
                        PendingCandidates = pendingCandidates,
                        TotalVotes = totalVotes,
                        VoterTurnout = totalVoters > 0 ? Math.Round((votedCount * 100.0) / totalVoters, 2) : 0
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // ✅ NEW: Get candidate details with photo
        // GET: api/admin/candidate-with-photo/{id}
        [HttpGet("candidate-with-photo/{id}")]
        public async Task<IActionResult> GetCandidateWithPhoto(int id)
        {
            try
            {
                var candidate = await _context.Candidates
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (candidate == null)
                    return NotFound(new { success = false, message = "Candidate not found" });

                var result = new
                {
                    candidate.Id,
                    candidate.FullName,
                    candidate.Age,
                    candidate.Region,
                    candidate.PartyAffiliation,
                    candidate.Email,
                    candidate.Phone,
                    candidate.Status,
                    candidate.IsApproved,
                    candidate.IsRejected,
                    candidate.ApplicationDate,
                    candidate.PhotoFilePath,
                    HasPhoto = !string.IsNullOrEmpty(candidate.PhotoFilePath),
                    VotesReceived = await _context.Votes.CountAsync(v => v.CandidateId == candidate.Id)
                };

                return Ok(new { success = true, candidate = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // ✅ NEW: Toggle voter account status
        // PUT: api/admin/toggle-voter-status/{id}
        [HttpPut("toggle-voter-status/{id}")]
        public async Task<IActionResult> ToggleVoterStatus(int id)
        {
            try
            {
                var voter = await _context.Users.FindAsync(id);
                if (voter == null)
                    return NotFound(new { success = false, message = "Voter not found" });

                voter.IsApproved = !voter.IsApproved;
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = $"Voter account {(voter.IsApproved ? "enabled" : "disabled")}",
                    isApproved = voter.IsApproved
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // =========================
        // ✅ INTERNAL HELPERS (UNCHANGED)
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