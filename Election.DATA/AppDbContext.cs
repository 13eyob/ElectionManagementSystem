using Election.DATA.Models;
using Microsoft.EntityFrameworkCore;

namespace Election.DATA
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Candidate> Candidates { get; set; } = null!;
    }
}