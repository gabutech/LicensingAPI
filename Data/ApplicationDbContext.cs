using Microsoft.EntityFrameworkCore;

namespace LicensingAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=Licensing.dat");
        }

        public DbSet<Models.License> Licenses { get; set; } 
    }
}
