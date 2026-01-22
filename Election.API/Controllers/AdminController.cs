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
        // 1. DASHBOARD STATISTICS (FIXED FOR FORM)
        // =========================

        // GET: api/admin/dashboard-statistics
        [HttpGet("dashboard-statistics")]
        public async Task<IActionResult> GetDashboardStatistics()
        {
            try
            {
                // Get all statistics with EXACT property names form expects
                var statistics = new
                {
                    // REQUIRED by frmAdminDashboard - EXACT NAMES
                    TotalUsers = await _context.Users.CountAsync(),
                    TotalVoters = await _context.Users.CountAsync(u => u.Role == "Voter"),
                    TotalCandidates = await _context.Candidates.CountAsync(),
                    TotalVotes = await _context.Votes.CountAsync(),
                    ApprovedCandidates = await _context.Candidates.CountAsync(c => c.Status == "Approved"),
                    PendingCandidates = await _context.Candidates.CountAsync(c => c.Status == "Pending"),
                    RejectedCandidates = await _context.Candidates.CountAsync(c => c.Status == "Rejected"),

                    // Additional stats (optional)
                    TotalAdmins = await _context.Users.CountAsync(u => u.Role == "Admin"),
                    VotedCount = await _context.Users.CountAsync(u => u.HasVoted),
                    VoterTurnout = await CalculateVoterTurnout()
                };

                return Ok(new
                {
                    success = true,
                    statistics // MUST be named "statistics" (form expects this)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // GET: api/admin/dashboard-stats (Compatibility endpoint)
        [HttpGet("dashboard-stats")]
        public async Task<IActionResult> GetDashboardStats()
        {
            return await GetDashboardStatistics();
        }

        private async Task<double> CalculateVoterTurnout()
        {
            var totalVoters = await _context.Users.CountAsync(u => u.Role == "Voter");
            var votedCount = await _context.Users.CountAsync(u => u.HasVoted);

            if (totalVoters == 0) return 0;
            return Math.Round((votedCount * 100.0) / totalVoters, 2);
        }

        // =========================
        // 2. ELECTION CONTROL
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
        // 3. USER MANAGEMENT
        // =========================

        // GET: api/admin/all-users
        [HttpGet("all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _context.Users
                    .OrderByDescending(u => u.CreatedAt)
                    .Select(u => new
                    {
                        Id = u.Id,
                        Username = u.Username,
                        FullName = u.FullName,
                        Email = u.Email,
                        Role = u.Role,
                        Region = u.Region,
                        Age = u.Age,
                        IsApproved = u.IsApproved,
                        HasVoted = u.HasVoted,
                        CreatedAt = u.CreatedAt,
                        //LastLogin = u.LastLogin
                    })
                    .ToListAsync();

                return Ok(new { success = true, users });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // PUT: api/admin/toggle-user-status/{id}
        [HttpPut("toggle-user-status/{id}")]
        public async Task<IActionResult> ToggleUserStatus(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                    return NotFound(new { success = false, message = "User not found" });

                user.IsApproved = !user.IsApproved;
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = $"User {user.Username} account {(user.IsApproved ? "enabled" : "disabled")}",
                    isApproved = user.IsApproved
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // =========================
        // 4. CANDIDATE DETAILS
        // =========================

        // GET: api/admin/candidate-details/{id}
        [HttpGet("candidate-details/{id}")]
        public async Task<IActionResult> GetCandidateDetails(int id)
        {
            try
            {
                var candidate = await _context.Candidates
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (candidate == null)
                    return NotFound(new { success = false, message = "Candidate not found" });

                var votes = await _context.Votes.CountAsync(v => v.CandidateId == id);
                var totalVotes = await _context.Votes.CountAsync();
                var percentage = totalVotes > 0 ? Math.Round((votes * 100.0) / totalVotes, 2) : 0;

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == candidate.Email);

                return Ok(new
                {
                    success = true,
                    candidate = new
                    {
                        // Required by form
                        Id = candidate.Id,
                        FullName = candidate.FullName,
                        Age = candidate.Age,
                        Region = candidate.Region,
                        PartyAffiliation = candidate.PartyAffiliation,
                        Email = candidate.Email,
                        Phone = candidate.Phone,
                        Status = candidate.Status,
                        IsApproved = candidate.IsApproved,
                        IsRejected = candidate.IsRejected,
                        ApplicationDate = candidate.ApplicationDate,
                        ApprovalDate = candidate.ApprovalDate,
                        AdminRemarks = candidate.AdminRemarks,
                        PhotoFilePath = candidate.PhotoFilePath,
                        ManifestoFilePath = candidate.ManifestoFilePath,
                        // Additional info
                        VotesReceived = votes,
                        VotePercentage = percentage,
                        Username = user?.Username ?? "N/A",
                        UserRole = user?.Role ?? "N/A"
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // =========================
        // 5. ELECTION RESULTS
        // =========================

        // GET: api/admin/election-results
        [HttpGet("election-results")]
        public async Task<IActionResult> GetElectionResults()
        {
            try
            {
                var totalVotes = await _context.Votes.CountAsync();

                var results = await _context.Candidates
                    .Where(c => c.Status == "Approved")
                    .Select(c => new
                    {
                        Id = c.Id,
                        FullName = c.FullName,
                        PartyAffiliation = c.PartyAffiliation,
                        Region = c.Region,
                        Votes = _context.Votes.Count(v => v.CandidateId == c.Id),
                        Percentage = totalVotes == 0 ? 0 :
                            Math.Round((_context.Votes.Count(v => v.CandidateId == c.Id) * 100.0) / totalVotes, 2)
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
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // =========================
        // 6. VOTE STATISTICS
        // =========================

        // GET: api/admin/total-votes
        [HttpGet("total-votes")]
        public async Task<IActionResult> GetTotalVotes()
        {
            var totalVotes = await _context.Votes.CountAsync();
            return Ok(new { success = true, totalVotes });
        }

        // GET: api/admin/vote-statistics
        [HttpGet("vote-statistics")]
        public async Task<IActionResult> GetVoteStatistics()
        {
            try
            {
                var totalVotes = await _context.Votes.CountAsync();
                var totalVoters = await _context.Users.CountAsync(u => u.Role == "Voter");
                var votedCount = await _context.Users.CountAsync(u => u.HasVoted);
                var turnout = totalVoters > 0 ? Math.Round((votedCount * 100.0) / totalVoters, 2) : 0;

                return Ok(new
                {
                    success = true,
                    totalVotes,
                    totalVoters,
                    votedCount,
                    voterTurnout = turnout
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // =========================
        // 7. COMPATIBILITY ENDPOINTS
        // =========================

        // GET: api/admin/voters
        [HttpGet("voters")]
        public async Task<IActionResult> GetAllVoters()
        {
            var voters = await _context.Users
                .Where(u => u.Role == "Voter")
                .Select(u => new
                {
                    u.Id,
                    u.Username,
                    u.Email,
                    u.FullName,
                    u.Region,
                    u.HasVoted,
                    u.IsApproved
                })
                .ToListAsync();

            return Ok(new { success = true, voters });
        }

        // =========================
        // 8. INTERNAL HELPERS
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