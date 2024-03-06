using Animeax.Models;
using Microsoft.EntityFrameworkCore;

namespace Animeax.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<AAnime> AAnime { get; set; }
        public DbSet<AnimeDLink> AnimeDLinks { get; set; }
    }
}
