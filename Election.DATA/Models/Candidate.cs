using System;
using System.ComponentModel.DataAnnotations;

namespace Election.DATA.Models
{
    public class Candidate
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string FullName { get; set; } = "";

        [Required]
        [Range(18, 100)]
        public int Age { get; set; }

        [Required]
        [MaxLength(100)]
        public string Region { get; set; } = "";

        [Required]
        [MaxLength(100)]
        public string PartyAffiliation { get; set; } = "";

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = "";

        [Required]
        [Phone]
        [MaxLength(20)]
        public string Phone { get; set; } = "";

        public string ManifestoFilePath { get; set; } = "";
        public string PhotoFilePath { get; set; } = "";

        public DateTime ApplicationDate { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Pending";

        // 🔥 NEW: Added for admin approval system
        public bool IsApproved { get; set; } = false;
    }
}