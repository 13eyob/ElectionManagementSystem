using Election.DATA.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Election.DATA
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Tables
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Candidate> Candidates { get; set; } = null!;
        public DbSet<ElectionSettings> ElectionSettings { get; set; } = null!;
        public DbSet<Vote> Votes { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ✅ ONE USER = ONE VOTE (DB LEVEL)
            modelBuilder.Entity<Vote>()
                .HasIndex(v => v.UserId)
                .IsUnique();

            // ✅ Foreign key: Vote → User
            modelBuilder.Entity<Vote>()
                .HasOne(v => v.User)
                .WithMany()
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ Foreign key: Vote → Candidate
            modelBuilder.Entity<Vote>()
                .HasOne(v => v.Candidate)
                .WithMany()
                .HasForeignKey(v => v.CandidateId)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ EXISTING SEED DATA (UNCHANGED)
            modelBuilder.Entity<ElectionSettings>().HasData(
                new ElectionSettings
                {
                    Id = 1,
                    IsElectionActive = false,
                    ElectionTitle = "General Election 2024",
                    VotingOpen = false,
                    RegistrationOpen = true,
                    CreatedDate = DateTime.UtcNow
                }
            );
        }
    }
}
