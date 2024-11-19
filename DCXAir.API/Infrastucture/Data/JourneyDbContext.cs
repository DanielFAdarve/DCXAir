namespace DCXAir.API.Infrastructure.Data
{
    using Microsoft.EntityFrameworkCore;
    using DCXAir.API.Domain.Entities;
    public class JourneyDbContext : DbContext
    {
        public JourneyDbContext(DbContextOptions<JourneyDbContext> options) : base(options) { }

        public DbSet<Journey> Journeys { get; set; }
        public DbSet<Flight> Fligths { get; set; }
        public DbSet<Transport> Transports { get; set; }
    }
}