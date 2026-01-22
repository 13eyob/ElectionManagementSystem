using Election.DATA;
using Election.DATA.Models;
using Microsoft.AspNetCore.Mvc;

namespace Election.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;

        public AuthController(AppDbContext db)
        {
            _db = db;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            var user = _db.Users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);

            if (user == null)
                return BadRequest("Invalid username or password");

            // ✅ SECURITY: Block login if user has already voted (Requirement #4)
            if (user.Role == "Voter" && user.HasVoted)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "🔒 This account has already completed voting and cannot login again.",
                    errorCode = "ACCOUNT_LOCKED_AFTER_VOTING"
                });
            }

            return Ok(new
            {
                success = true,
                userId = user.Id,  // ✅ Added for vote tracking
                fullName = user.FullName,
                role = user.Role,
                isApproved = user.IsApproved,
                email = user.Email,
                region = user.Region,  // ✅ Added for auto-fill in candidate application
                hasVoted = user.HasVoted  // ✅ Added for voting status tracking
            });
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterModel model)
        {
            if (_db.Users.Any(u => u.Username == model.Username))
                return BadRequest("Username already exists");

            var user = new User
            {
                FullName = model.FullName,
                Username = model.Username,
                Password = model.Password,
                Email = model.Email,
                Age = model.Age,
                Region = model.Region,
                Role = model.Role,
                IsApproved = model.Role == "Voter",
                HasVoted = false,
                CreatedAt = DateTime.Now
            };

            _db.Users.Add(user);
            _db.SaveChanges();

            return Ok(new { message = "Registration successful" });
        }
    }

    public class LoginModel
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }

    public class RegisterModel
    {
        public string FullName { get; set; } = "";
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string Email { get; set; } = "";
        public int Age { get; set; }
        public string Region { get; set; } = "";
        public string Role { get; set; } = "Voter";
    }
}