using aspnet_logger_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace aspnet_logger_backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Logentry> Logentrys { get; set; }
    }
}
