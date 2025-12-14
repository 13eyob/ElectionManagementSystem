using Election.DATA;
using Election.DATA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace Election.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ILogger<VoteController> _logger;

        public VoteController(AppDbContext db, ILogger<VoteController> logger)
        {
            _db = db;
            _logger = logger;
        }

        // ✅ Check if user has already voted
        [HttpGet("hasvoted/{userId}")]
        public async Task<IActionResult> HasVoted(int userId)
        {
            try
            {
                var user = await _db.Users.FindAsync(userId);
                if (user == null)
                {
                    return Ok(new { success = true, hasVoted = false, message = "User not found" });
                }

                // ✅ Also check Votes table to ensure consistency
                var voteExists = await _db.Votes.AnyAsync(v => v.UserId == userId);

                // Fix inconsistency if found
                if (user.HasVoted != voteExists)
                {
                    _logger.LogWarning($"Inconsistency detected for user {userId}: HasVoted={user.HasVoted}, VoteExists={voteExists}");
                    user.HasVoted = voteExists;
                    await _db.SaveChangesAsync();
                }

                return Ok(new { success = true, hasVoted = user.HasVoted });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in HasVoted");
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // ✅ Submit a vote - FIXED DUPLICATE ISSUE
        [HttpPost("submit")]
        public async Task<IActionResult> SubmitVote([FromBody] VoteRequest request)
        {
            _logger.LogInformation($"SubmitVote called: UserId={request.UserId}, CandidateId={request.CandidateId}");

            try
            {
                // 1. Validate user
                var user = await _db.Users.FindAsync(request.UserId);
                if (user == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = $"User ID {request.UserId} not found.",
                        errorCode = "USER_NOT_FOUND"
                    });
                }

                // 2. Check if already voted (in Votes table)
                var existingVote = await _db.Votes
                    .FirstOrDefaultAsync(v => v.UserId == request.UserId);

                if (existingVote != null)
                {
                    _logger.LogWarning($"User {request.UserId} already voted on {existingVote.VoteDate}");

                    // Ensure user.HasVoted is true
                    if (!user.HasVoted)
                    {
                        user.HasVoted = true;
                        await _db.SaveChangesAsync();
                    }

                    return BadRequest(new
                    {
                        success = false,
                        message = "You have already voted in this election.",
                        errorCode = "ALREADY_VOTED",
                        voteDate = existingVote.VoteDate.ToString("yyyy-MM-dd HH:mm"),
                        cannotVoteAgain = true
                    });
                }

                // 3. Check if user.HasVoted flag is true (inconsistent state)
                if (user.HasVoted)
                {
                    _logger.LogWarning($"User {request.UserId} has HasVoted=true but no vote in table. Fixing...");
                    user.HasVoted = false; // Reset to allow voting
                    await _db.SaveChangesAsync();
                }

                // 4. Validate candidate
                var candidate = await _db.Candidates
                    .FirstOrDefaultAsync(c => c.Id == request.CandidateId && c.IsApproved);

                if (candidate == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = $"Candidate ID {request.CandidateId} not found or not approved.",
                        errorCode = "CANDIDATE_NOT_FOUND"
                    });
                }

                // 5. Create and save vote
                var vote = new Vote
                {
                    UserId = request.UserId,
                    CandidateId = request.CandidateId,
                    VoteDate = DateTime.Now
                };

                // Update user flag
                user.HasVoted = true;

                // Add to database
                _db.Votes.Add(vote);

                // Save with retry for concurrency
                try
                {
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateException dbEx) when (IsDuplicateError(dbEx))
                {
                    _logger.LogWarning(dbEx, "Duplicate vote detected after validation");

                    // Double-check if vote was actually created
                    var doubleCheckVote = await _db.Votes
                        .FirstOrDefaultAsync(v => v.UserId == request.UserId);

                    if (doubleCheckVote != null)
                    {
                        return BadRequest(new
                        {
                            success = false,
                            message = "Your vote was already recorded.",
                            errorCode = "VOTE_ALREADY_RECORDED",
                            voteId = doubleCheckVote.Id
                        });
                    }

                    throw; // Re-throw if it's not actually a duplicate
                }

                _logger.LogInformation($"Vote saved successfully: VoteId={vote.Id}, User={user.Email}, Candidate={candidate.FullName}");

                return Ok(new
                {
                    success = true,
                    message = "✅ Vote submitted successfully!",
                    voteId = vote.Id,
                    candidateName = candidate.FullName,
                    party = candidate.PartyAffiliation,
                    voteDate = vote.VoteDate.ToString("yyyy-MM-dd HH:mm:ss")
                });
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database error in SubmitVote");

                // Check for specific SQL errors
                if (dbEx.InnerException is SqlException sqlEx)
                {
                    switch (sqlEx.Number)
                    {
                        case 2627: // Primary key violation
                        case 2601: // Unique constraint
                            return Conflict(new
                            {
                                success = false,
                                message = "Duplicate vote detected by database.",
                                errorCode = "DB_DUPLICATE_VOTE",
                                solution = "Please refresh and check if your vote was recorded."
                            });

                        case 547: // Foreign key violation
                            return BadRequest(new
                            {
                                success = false,
                                message = "Invalid user or candidate reference.",
                                errorCode = "INVALID_REFERENCE"
                            });
                    }
                }

                return StatusCode(500, new
                {
                    success = false,
                    message = "Database error while saving vote.",
                    errorCode = "DATABASE_ERROR"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in SubmitVote");
                return StatusCode(500, new
                {
                    success = false,
                    message = "An unexpected error occurred.",
                    errorCode = "UNEXPECTED_ERROR"
                });
            }
        }

        // ✅ Force reset user's voting status (for testing/admin)
        [HttpPost("reset-user/{userId}")]
        public async Task<IActionResult> ResetUserVote(int userId)
        {
            try
            {
                var user = await _db.Users.FindAsync(userId);
                if (user == null)
                    return NotFound(new { success = false, message = "User not found" });

                // Remove any votes by this user
                var userVotes = await _db.Votes.Where(v => v.UserId == userId).ToListAsync();
                if (userVotes.Any())
                {
                    _db.Votes.RemoveRange(userVotes);
                }

                // Reset HasVoted flag
                user.HasVoted = false;
                await _db.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = $"User {userId} voting status reset. Can vote again now.",
                    votesRemoved = userVotes.Count
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting user vote");
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // ✅ Get all votes (for admin dashboard)
        [HttpGet("all")]
        public async Task<IActionResult> GetAllVotes()
        {
            try
            {
                var votes = await _db.Votes
                    .Include(v => v.Candidate)
                    .Select(v => new
                    {
                        v.Id,
                        UserId = v.UserId,
                        CandidateId = v.CandidateId,
                        CandidateName = v.Candidate.FullName,
                        Party = v.Candidate.PartyAffiliation,
                        VoteDate = v.VoteDate.ToString("yyyy-MM-dd HH:mm:ss")
                    })
                    .OrderByDescending(v => v.VoteDate)
                    .ToListAsync();

                return Ok(new { success = true, votes });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllVotes");
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // ✅ Get vote statistics (for admin)
        [HttpGet("statistics")]
        public async Task<IActionResult> GetVoteStatistics()
        {
            try
            {
                var totalVotes = await _db.Votes.CountAsync();
                var totalVoters = await _db.Users.CountAsync(u => u.Role == "Voter");
                var votersWhoHaveVoted = await _db.Users.CountAsync(u => u.Role == "Voter" && u.HasVoted);
                var votersWhoHaventVoted = totalVoters - votersWhoHaveVoted;

                var votesByParty = await _db.Votes
                    .Include(v => v.Candidate)
                    .GroupBy(v => v.Candidate.PartyAffiliation)
                    .Select(g => new
                    {
                        Party = g.Key,
                        Votes = g.Count()
                    })
                    .OrderByDescending(g => g.Votes)
                    .ToListAsync();

                return Ok(new
                {
                    success = true,
                    totalVotes,
                    totalVoters,
                    votersWhoHaveVoted,
                    votersWhoHaventVoted,
                    votesByParty
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetVoteStatistics");
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // ✅ RESET ALL VOTING (For admin to reset for new election)
        [HttpPost("reset")]
        public async Task<IActionResult> ResetVoting()
        {
            try
            {
                // Reset all users' HasVoted flag
                var voters = await _db.Users.Where(u => u.Role == "Voter").ToListAsync();
                foreach (var voter in voters)
                {
                    voter.HasVoted = false;
                }

                // Clear votes table
                _db.Votes.RemoveRange(_db.Votes);

                await _db.SaveChangesAsync();

                _logger.LogInformation("Voting system reset by admin");

                return Ok(new
                {
                    success = true,
                    message = "✅ Voting system has been reset. All votes cleared and users can vote again.",
                    usersReset = voters.Count,
                    votesCleared = "All"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting voting");
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // ✅ DEBUG: Check database state
        [HttpGet("debug")]
        public async Task<IActionResult> Debug()
        {
            try
            {
                var info = new
                {
                    Database = new
                    {
                        Name = _db.Database.GetDbConnection().Database,
                        CanConnect = _db.Database.CanConnect(),
                        Server = _db.Database.GetDbConnection().DataSource
                    },
                    Counts = new
                    {
                        Users = await _db.Users.CountAsync(),
                        Candidates = await _db.Candidates.CountAsync(),
                        Votes = await _db.Votes.CountAsync(),
                        ApprovedCandidates = await _db.Candidates.CountAsync(c => c.IsApproved)
                    },
                    RecentVotes = await _db.Votes
                        .OrderByDescending(v => v.VoteDate)
                        .Take(5)
                        .Select(v => new { v.Id, v.UserId, v.CandidateId, v.VoteDate })
                        .ToListAsync(),
                    HasVotedIssues = await _db.Users
                        .Where(u => u.HasVoted && !_db.Votes.Any(v => v.UserId == u.Id))
                        .Select(u => new { u.Id, u.Email })
                        .ToListAsync()
                };

                return Ok(new { success = true, debug = info });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, error = ex.Message });
            }
        }

        // Helper method to detect duplicate errors
        private bool IsDuplicateError(DbUpdateException ex)
        {
            return ex.InnerException is SqlException sqlEx &&
                   (sqlEx.Number == 2627 || sqlEx.Number == 2601);
        }
    }

    public class VoteRequest
    {
        public int UserId { get; set; }
        public int CandidateId { get; set; }
    }
}