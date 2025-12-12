using System;
using System.ComponentModel.DataAnnotations;

namespace Election.DATA.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; } = "";

        [Required]
        public string Username { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";

        public string Email { get; set; } = "";
        public int Age { get; set; }
        public string Region { get; set; } = "";

        [Required]
        public string Role { get; set; } = "Voter";

        public bool IsApproved { get; set; }
        public bool HasVoted { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}