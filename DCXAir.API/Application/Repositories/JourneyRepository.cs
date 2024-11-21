namespace DCXAir.API.Application.Repositories
{
    using DCXAir.API.Infrastructure;
    using DCXAir.API.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using DCXAir.API.Infrastructure.Data;

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

        public async Task AddJourneyAsync(Journey journey)
        {
            await _context.Journeys.AddAsync(journey);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Journey>> GetJourneysByOriginAndDestinationAsync(string origin, string destination)
        {
            return await _context.Journeys
                .Where(j => j.Fligths.Any(f => f.Origin == origin && f.Destination == destination))
                .Include(j => j.Fligths)
                .ThenInclude(f => f.Transport)
                .ToListAsync();
        }


        public async Task<List<Journey>> GetJourneysWithStopsAsync(string origin, string destination)
        {
            var journeysWithStops = await _context.Flights
                .Include(f1 => f1.Transport)
                .Join(
                    _context.Flights.Include(f2 => f2.Transport),  
                    f1 => f1.Destination,                         
                    f2 => f2.Origin,                              
                    (f1, f2) => new { FirstFlight = f1, SecondFlight = f2 }
                )
                .Where(joined =>
                    joined.FirstFlight.Origin == origin &&       
                    joined.SecondFlight.Destination == destination 
                )
                .Select(joined => new Journey
                {
                    Fligths = new List<Flight> {
                    joined.FirstFlight,
                    joined.SecondFlight
                    },
                })
                .ToListAsync();

            return journeysWithStops;
        }
    }
}
