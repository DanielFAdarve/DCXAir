namespace DCXAir.API.Application.Repositories
{
    using DCXAir.API.Infrastructure;
    using DCXAir.API.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using DCXAir.API.Infrastructure.Data;
    public class JourneyRepository
    {
        private readonly JourneyDbContext _context;

        public JourneyRepository(JourneyDbContext context)
        {
            _context = context;
        }
        public async Task<List<Journey>> GetAllJourneysAsync()
        {
            return await _context.Journeys.Include(j => j.Fligths).ThenInclude(f => f.Transport).ToListAsync();
        }
        public async Task AddJourneyAsync(Journey journey)
        {
            await _context.Journeys.AddAsync(journey);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Journey>> GetJourneysByOriginAndDestinationAsync(string origin, string destination)
        {
            return await _context.Journeys
                .Where(j => j.Fligths.Any(f => f.Origin == origin && f.Destination == destination))
                .ToListAsync();
        }
    }
}
