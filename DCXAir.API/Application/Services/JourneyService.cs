namespace DCXAir.API.Application.Services
{
    using DCXAir.API.Application.DTOs;
    using DCXAir.API.Application.Interfaces;
    using System.Linq;

    public class JourneyService : IJourneyService
    {
        private readonly List<JourneyDto> _journeys;

        public JourneyService(List<JourneyDto> journeys)
        {
            _journeys = journeys;
        }
        public List<JourneyDto> GetFligths(string origin, string destination, string type, string currency)
        {
            return _journeys
                .Where(j => j.Flights.Any(f => f.Origin == origin && f.Destination == destination))
                .Select(j => new JourneyDto
                {
                    Type = type,
                    Flights = j.Flights
                        .Where(f => f.Origin == origin && f.Destination == destination)
                        .Select(f => new FlightDto
                        {
                            Origin = f.Origin,
                            Destination = f.Destination,
                            Price = f.Price,
                            Transport = f.Transport
                        })
                        .ToList(),
                    TotalPrice = j.Flights
                        .Where(f => f.Origin == origin && f.Destination == destination)
                        .Sum(f => f.Price) 
                })
                .ToList();
        }
    }
}