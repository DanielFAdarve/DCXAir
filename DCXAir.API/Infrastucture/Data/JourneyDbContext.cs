namespace DCXAir.API.Infrastructure.Data {
    using DCXAir.API.Domain.Entities;
    using Microsoft.EntityFrameworkCore;

    public class JourneyDbContext : DbContext
    {

        public JourneyDbContext(DbContextOptions<JourneyDbContext> options) : base(options) { }

        public DbSet<Journey> Journeys { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Transport> Transports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Journey>()
                .HasMany(j => j.Fligths)
                .WithOne()
                .HasForeignKey(f => f.Id); 

         
            modelBuilder.Entity<Flight>()
                .HasOne(f => f.Transport)
                .WithOne()
                .HasForeignKey<Flight>(t => t.Id); 
        }
    }
}