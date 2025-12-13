using Election.DATA;
using Election.DATA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

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

        // ============ 🗳️ CRITICAL: Get approved candidates for VOTER DASHBOARD ============
        [HttpGet("approved")]
        public async Task<IActionResult> GetApprovedCandidates()
        {
            try
            {
                var candidates = await _db.Candidates
                    .Where(c => c.IsApproved && c.Status == "Approved")
                    .OrderBy(c => c.PartyAffiliation)
                    .Select(c => new
                    {
                        Id = c.Id,
                        Name = c.FullName,           // Changed from FullName to Name
                        Party = c.PartyAffiliation,  // Changed from PartyAffiliation to Party
                        Region = c.Region,
                        Age = c.Age,
                        IsApproved = c.IsApproved
                    })
                    .ToListAsync();

                return Ok(candidates);  // ✅ Returns direct list, no wrapper
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // ============ 🔥 Candidate Statistics Endpoint ============
        [HttpGet("statistics")]
        public async Task<IActionResult> GetCandidateStatistics()
        {
            try
            {
                var candidates = await _db.Candidates.ToListAsync();

                var statistics = new
                {
                    TotalCandidates = candidates.Count,
                    ApprovedCandidates = candidates.Count(c => c.IsApproved && c.Status == "Approved"),
                    PendingCandidates = candidates.Count(c => !c.IsApproved && c.Status == "Pending"),
                    RejectedCandidates = candidates.Count(c => c.Status == "Rejected"),
                    TodayRegistrations = candidates.Count(c => c.ApplicationDate.Date == DateTime.Today),
                    ThisWeekRegistrations = candidates.Count(c => c.ApplicationDate >= DateTime.Today.AddDays(-7))
                };

                return Ok(new { success = true, statistics });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // ============ 🔥 Get all candidates for admin ============
        [HttpGet("all")]
        public async Task<IActionResult> GetAllCandidates()
        {
            try
            {
                var candidates = await _db.Candidates
                    .OrderByDescending(c => c.ApplicationDate)
                    .Select(c => new
                    {
                        c.Id,
                        c.FullName,
                        c.PartyAffiliation,
                        c.Region,
                        c.Age,
                        c.Email,
                        c.Phone,
                        c.Status,
                        c.IsApproved,
                        c.ApplicationDate,
                        HasPhoto = !string.IsNullOrEmpty(c.PhotoFilePath),
                        HasManifesto = !string.IsNullOrEmpty(c.ManifestoFilePath)
                    })
                    .ToListAsync();

                return Ok(new { success = true, candidates });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // ============ 🔥 Get candidate details by ID ============
        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetCandidateDetails(int id)
        {
            try
            {
                var candidate = await _db.Candidates
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (candidate == null)
                {
                    return NotFound(new { success = false, message = "Candidate not found" });
                }

                // Get photo as base64 for display
                string? photoBase64 = null;
                if (!string.IsNullOrEmpty(candidate.PhotoFilePath))
                {
                    var physicalPath = Path.Combine(_environment.WebRootPath, candidate.PhotoFilePath.TrimStart('/'));
                    if (System.IO.File.Exists(physicalPath))
                    {
                        byte[] imageBytes = await System.IO.File.ReadAllBytesAsync(physicalPath);
                        photoBase64 = Convert.ToBase64String(imageBytes);
                    }
                }

                // Get manifesto content
                string? manifestoContent = null;
                if (!string.IsNullOrEmpty(candidate.ManifestoFilePath))
                {
                    var physicalPath = Path.Combine(_environment.WebRootPath, candidate.ManifestoFilePath.TrimStart('/'));
                    if (System.IO.File.Exists(physicalPath))
                    {
                        manifestoContent = await System.IO.File.ReadAllTextAsync(physicalPath);
                    }
                }

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
                    candidate.ApplicationDate,
                    PhotoBase64 = photoBase64,
                    ManifestoContent = manifestoContent,
                    PhotoFileName = Path.GetFileName(candidate.PhotoFilePath),
                    ManifestoFileName = Path.GetFileName(candidate.ManifestoFilePath)
                };

                return Ok(new { success = true, candidate = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // ============ 🔥 Approve candidate ============
        [HttpPut("approve/{id}")]
        public async Task<IActionResult> ApproveCandidate(int id)
        {
            try
            {
                var candidate = await _db.Candidates.FindAsync(id);
                if (candidate == null)
                {
                    return NotFound(new { success = false, message = "Candidate not found" });
                }

                candidate.Status = "Approved";
                candidate.IsApproved = true;
                await _db.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = $"Candidate {candidate.FullName} approved successfully!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // ============ 🔥 Reject candidate ============
        [HttpPut("reject/{id}")]
        public async Task<IActionResult> RejectCandidate(int id)
        {
            try
            {
                var candidate = await _db.Candidates.FindAsync(id);
                if (candidate == null)
                {
                    return NotFound(new { success = false, message = "Candidate not found" });
                }

                candidate.Status = "Rejected";
                candidate.IsApproved = false;
                await _db.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = $"Candidate {candidate.FullName} has been rejected."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // ============ ✅ YOUR EXISTING ENDPOINTS (KEEP THESE) ============

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitCandidate([FromForm] CandidateRequest model)
        {
            try
            {
                // 1. VALIDATE TEXT FIELDS
                if (string.IsNullOrWhiteSpace(model.FullName))
                    return BadRequest(new { success = false, message = "Full Name is required" });

                if (model.Age < 18 || model.Age > 100)
                    return BadRequest(new { success = false, message = "Age must be 18-100" });

                if (string.IsNullOrWhiteSpace(model.Region))
                    return BadRequest(new { success = false, message = "Region is required" });

                if (string.IsNullOrWhiteSpace(model.PartyAffiliation))
                    return BadRequest(new { success = false, message = "Party/Affiliation is required" });

                if (string.IsNullOrWhiteSpace(model.Email) || !model.Email.Contains("@"))
                    return BadRequest(new { success = false, message = "Valid Email is required" });

                if (string.IsNullOrWhiteSpace(model.Phone))
                    return BadRequest(new { success = false, message = "Phone is required" });

                // 2. VALIDATE FILES
                if (model.ManifestoFile == null || model.ManifestoFile.Length == 0)
                    return BadRequest(new { success = false, message = "Manifesto file is required" });

                if (model.PhotoFile == null || model.PhotoFile.Length == 0)
                    return BadRequest(new { success = false, message = "Photo file is required" });

                // 3. CHECK IF EMAIL EXISTS
                var existing = await _db.Candidates.FirstOrDefaultAsync(c => c.Email == model.Email);
                if (existing != null)
                    return BadRequest(new { success = false, message = "This email is already registered" });

                // 4. SAVE FILES
                string manifestoPath = "";
                string photoPath = "";

                try
                {
                    // Save manifesto
                    manifestoPath = await SaveFile(model.ManifestoFile, "manifestos",
                        new[] { ".pdf", ".doc", ".docx", ".txt" });

                    // Save photo
                    photoPath = await SaveFile(model.PhotoFile, "photos",
                        new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif" });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { success = false, message = ex.Message });
                }

                // 5. SAVE TO DATABASE
                var candidate = new Candidate
                {
                    FullName = model.FullName.Trim(),
                    Age = model.Age,
                    Region = model.Region.Trim(),
                    PartyAffiliation = model.PartyAffiliation.Trim(),
                    Email = model.Email.Trim(),
                    Phone = model.Phone.Trim(),
                    ManifestoFilePath = manifestoPath,
                    PhotoFilePath = photoPath,
                    ApplicationDate = DateTime.Now,
                    Status = "Pending",
                    IsApproved = false
                };

                _db.Candidates.Add(candidate);
                await _db.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = "Candidate application submitted successfully!",
                    candidateId = candidate.Id,
                    fullName = candidate.FullName
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Server error: {ex.Message}" });
            }
        }

        private async Task<string> SaveFile(IFormFile file, string folder, string[] allowedExtensions)
        {
            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExtension))
                throw new Exception($"Invalid {folder} file type. Allowed: {string.Join(", ", allowedExtensions)}");

            if (file.Length > 10 * 1024 * 1024) // 10MB
                throw new Exception($"File too large. Maximum size is 10MB.");

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", folder);
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/{folder}/{fileName}";
        }

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
                    Status = candidate.Status,
                    IsApproved = candidate.IsApproved
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

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

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCandidate(int id)
        {
            try
            {
                var candidate = await _db.Candidates.FindAsync(id);
                if (candidate == null)
                {
                    return NotFound(new { success = false, message = "Candidate not found" });
                }

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

    public class CandidateRequest
    {
        public string FullName { get; set; } = "";
        public int Age { get; set; }
        public string Region { get; set; } = "";
        public string PartyAffiliation { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public IFormFile ManifestoFile { get; set; }
        public IFormFile PhotoFile { get; set; }
    }

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