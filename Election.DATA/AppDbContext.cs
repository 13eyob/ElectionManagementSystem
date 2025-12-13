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

        // Existing tables
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Candidate> Candidates { get; set; } = null!;
        public DbSet<ElectionSettings> ElectionSettings { get; set; } = null!;

        // 🔥 NEW: Votes table
        public DbSet<Vote> Votes { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🔐 Prevent double voting (ONE vote per user)
            modelBuilder.Entity<Vote>()
                .HasIndex(v => v.UserId)
                .IsUnique();

            // 🗳 Seed initial election settings
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
