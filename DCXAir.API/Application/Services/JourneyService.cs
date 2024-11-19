

namespace DCXAir.API.Application.Services
{
    using DCXAir.API.Application.Repositories;
    using DCXAir.API.Application.DTOs;
    using DCXAir.API.Domain.Entities;
    using DCXAir.API.Application.Interfaces;
    public class JourneyService : IJourneyService
    {
        private readonly JourneyRepository _journeyRepository;

        public JourneyService(JourneyRepository journeyRepository)
        {
            _journeyRepository = journeyRepository;
        }

        public async Task<ApiResponseDto> GetFligths(string origin, string destination, string type, string currency)
        {
            var journeys = new List<JourneyDto>();

            if (type == "one-way")
            {
                journeys = await GetOneWayFlights(origin, destination, type, currency);
            }
            else if (type == "round-trip")
            {
                journeys = await GetRoundTripFlights(origin, destination, type, currency);
            }

            var response = new ApiResponseDto
            {
                Origin = origin,
                Destination = destination,
                Currency = currency,
                TotalRoutes = journeys.Count,
                Journeys = journeys
            };

            return response;
        }

        public async Task<ApiResponseDto> GetOneWayFlights(string origin, string destination,string type, string currency)
        {
            var journeys = await _journeyRepository.GetJourneysByOriginAndDestinationAsync(origin, destination);

            // Lógica para mapear a JourneyDto, etc.

            return mappedJourneys;
        }

        public async Task<ApiResponseDto> GetRoundTripFlights(string origin, string destination,string type, string currency)
        {
            var outboundJourneys = await GetOneWayFlights(origin, destination,type, currency);
            var returnJourneys = await GetOneWayFlights(destination, origin,type, currency);

            // Lógica para combinar los viajes de ida y vuelta y mapear a JourneyDto

            return roundTripJourneys;
        }
    }
}