namespace DCXAir.API.Application.Repositories
{
    using DCXAir.API.Domain.Entities;
    using DCXAir.API.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class JourneyRepository : IJourneyRepository
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
        public async Task<List<Journey>> GetJourneysByOriginAndDestinationAsync(string origin, string destination)
        {
            return await _context.Journeys
                .Include(j => j.Fligths) 
                .Where(j => j.Fligths.Any(f => f.Origin == origin && f.Destination == destination))
                .ToListAsync();
        }

        public async Task<List<Journey>> GetJourneysWithStopsAsync(string origin, string destination)
        {
            return await _context.Journeys
                .Include(j => j.Fligths) 
                .Where(j => j.Fligths.Any(f => f.Origin == origin) && j.Fligths.Any(f => f.Destination == destination))
                .ToListAsync();
        }
    }
}
