using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Election.DATA.Models
{
    [Index(nameof(UserId), IsUnique = true, Name = "IX_Votes_UserId_Unique")]
    public class Vote
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int CandidateId { get; set; }

        [Required]
        public DateTime VoteDate { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        [ForeignKey(nameof(CandidateId))]
        public virtual Candidate Candidate { get; set; } = null!;
    }
}