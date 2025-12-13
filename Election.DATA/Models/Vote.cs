using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Election.DATA.Models
{
    public class Vote
    {
        [Key]
        public int Id { get; set; }

        // One voter (user)
        [Required]
        public int UserId { get; set; }

        // One candidate
        [Required]
        public int CandidateId { get; set; }

        public DateTime VoteDate { get; set; } = DateTime.Now;

        // Navigation (optional but good)
        [ForeignKey("CandidateId")]
        public Candidate? Candidate { get; set; }
    }
}
