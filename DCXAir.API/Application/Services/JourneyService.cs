namespace DCXAir.API.Application.Services
{
    using DCXAir.API.Application.Repositories;
    using DCXAir.API.Application.DTOs;
    using DCXAir.API.Domain.Entities;
    using DCXAir.API.Application.Interfaces;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    public class JourneyService : IJourneyService
    {
        private readonly IJourneyRepository _journeyRepository;
        private readonly IMapper _mapper;

        public JourneyService(IJourneyRepository journeyRepository, IMapper mapper)
        {
            _journeyRepository = journeyRepository;
            _mapper = mapper;
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


        public async Task<List<JourneyDto>> GetOneWayFlights(string origin, string destination, string type, string currency)
        {
            var directJourneys = await _journeyRepository.GetJourneysByOriginAndDestinationAsync(origin, destination);

            var directJourneyDtos = directJourneys.Select(j => new JourneyDto
            {
                Type = "One-way direct",
                Flights = j.Fligths?.Select(f => new FlightDto
                {
                    Origin = f.Origin,
                    Destination = f.Destination,
                    Price = f.Price,
                    Currency = currency,
                            Transport = f.Transport != null ? new List<TransportDto>
                {
                    new TransportDto
                    {
                        FlightCarrier = f.Transport.FlightCarrier,
                        FlightNumber = f.Transport.FlightNumber
                    }
                } : new List<TransportDto>()
                }).ToList() ?? new List<FlightDto>(),
                TotalPrice = j.Price
            }).ToList();

            var connectingJourneys = await GetFlightsWithStops(origin, destination, currency);

            directJourneyDtos.AddRange(connectingJourneys);

            return directJourneyDtos;
        }

        public async Task<List<JourneyDto>> GetRoundTripFlights(string origin, string destination, string type, string currency)
        {
            var outboundJourneys = await GetOneWayFlights(origin, destination, type, currency);

            var returnJourneys = await GetOneWayFlights(destination, origin, type, currency);

            var roundTripJourneys = new List<JourneyDto>();

            foreach (var outbound in outboundJourneys)
            {
                foreach (var inbound in returnJourneys)
                {
                    var roundTripJourney = new JourneyDto
                    {
                        Type = "Round-trip",
                        Flights = outbound.Flights.Concat(inbound.Flights).ToList(),
                        TotalPrice = outbound.TotalPrice + inbound.TotalPrice
                    };
                    roundTripJourneys.Add(roundTripJourney);
                }
            }

            return roundTripJourneys;
        }

        public async Task<List<JourneyDto>> GetFlightsWithStops(string origin, string destination, string currency)
        {
            var connectingJourneys = await _journeyRepository.GetJourneysWithStopsAsync(origin, destination);

            var journeyDtos = connectingJourneys.Select(j => new JourneyDto
            {
                Type = "Connecting",
                Flights = j.Fligths?.Select(f => new FlightDto
                {
                    Origin = f.Origin,
                    Destination = f.Destination,
                    Price = f.Price,
                    Currency = currency,
                    Transport = f.Transport != null ? new List<TransportDto>
            {
                new TransportDto
                {
                    FlightCarrier = f.Transport.FlightCarrier,
                    FlightNumber = f.Transport.FlightNumber
                }
            } : new List<TransportDto>()
                }).ToList() ?? new List<FlightDto>(),
                TotalPrice = j.Price
            }).ToList();

            return journeyDtos;
        }

    }
}