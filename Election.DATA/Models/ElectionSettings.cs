using System;
using System.ComponentModel.DataAnnotations;

namespace Election.DATA.Models
{
    public class ElectionSettings
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public bool IsElectionActive { get; set; } = false;

        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        [Required]
        [MaxLength(200)]
        public string ElectionTitle { get; set; } = "General Election";

        public bool VotingOpen { get; set; } = false;
        public bool RegistrationOpen { get; set; } = true;

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
    }
}