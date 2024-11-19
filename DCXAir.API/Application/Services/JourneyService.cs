using DCXAir.API.Application.DTOs;
using DCXAir.API.Application.Interfaces;
using DCXAir.API.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace DCXAir.API.Application.Services
{
    public class JourneyService : IJourneyService
    {
        private readonly List<Journey> _journeys;

        public JourneyService(List<Journey> journeys)
        {
            _journeys = journeys;
        }

        public ApiResponseDto GetFligths(string origin, string destination, string type, string currency)
        {
            var journeys = new ApiResponseDto();

            if (type == "one-way")
            {
                journeys = GetOneWayFlights(origin, destination, type, currency);
            }
            else if (type == "round-trip")
            {
                journeys = GetRoundTripFlights(origin, destination, type, currency);
            }

            var response = new ApiResponseDto
            {
                Origin = origin,
                Destination = destination,
                Currency = currency,
                TotalRoutes = journeys.TotalRoutes, 
                Journeys = journeys.Journeys
            };

            return response;
        }



        public ApiResponseDto GetOneWayFlights(string origin, string destination, string type, string currency)
        {
            var journeys = new List<JourneyDto>();

            var directJourneys = _journeys.Where(j => j.Fligths.Any(f => f.Origin == origin && f.Destination == destination)).ToList();
            journeys.AddRange(directJourneys.Select(j => new JourneyDto
            {
                Type = $"{type}-direct",
                Flights = j.Fligths.Select(f => new FlightDto
                {
                    Origin = f.Origin,
                    Destination = f.Destination,
                    Price = f.Price,
                    Currency = currency,
                    Transport = new List<TransportDto>
            {
                new TransportDto
                {
                    FlightCarrier = f.Transport.FlightCarrier,
                    FlightNumber = f.Transport.FlightNumber
                }
            }
                }).ToList(),
                TotalPrice = j.Price
            }));

            var connectingJourneys = GetFlightsWithStops(origin, destination, currency);
            journeys.AddRange(connectingJourneys.Select(j => new JourneyDto
            {
                Type = $"{type}-connecting",
                Flights = j.Fligths.Select(f => new FlightDto
                {
                    Origin = f.Origin,
                    Destination = f.Destination,
                    Price = f.Price,
                    Currency = currency,
                    Transport = new List<TransportDto>
            {
                new TransportDto
                {
                    FlightCarrier = f.Transport.FlightCarrier,
                    FlightNumber = f.Transport.FlightNumber
                }
            }
                }).ToList(),
                TotalPrice = j.Price
            }));

            return new ApiResponseDto
            {
                Origin = origin,
                Destination = destination,
                Currency = currency,
                TotalRoutes = journeys.Count,
                Journeys = journeys
            };
        }

        public List<Journey> GetFlightsWithStops(string origin, string destination, string currency)
        {
            var journeys = new List<Journey>();

            var connectingJourneys = _journeys.Where(j => j.Fligths.Any(f => f.Origin == origin)).ToList();
            foreach (var journey in connectingJourneys)
            {
                foreach (var flight in journey.Fligths)
                {
                    var nextFlights = _journeys
                        .Where(j2 => j2.Fligths.Any(f2 => f2.Origin == flight.Destination && f2.Destination == destination))
                        .ToList();

                    foreach (var nextJourney in nextFlights)
                    {
                        var newJourney = new Journey
                        {
                            Fligths = journey.Fligths.Concat(nextJourney.Fligths).ToList()
                        };
                        journeys.Add(newJourney);
                    }
                }
            }

            return journeys;
        }

        public ApiResponseDto GetRoundTripFlights(string origin, string destination, string type, string currency)
        {
            var outboundJourneys = GetOneWayFlights(origin, destination, type, currency).Journeys;
            var returnJourneys = GetOneWayFlights(destination, origin, type, currency).Journeys;

            var roundTripJourneys = new List<JourneyDto>();

            foreach (var outbound in outboundJourneys)
            {
                foreach (var inbound in returnJourneys)
                {
                    var roundTripJourney = new JourneyDto
                    {
                        Type = type,
                        Flights = outbound.Flights.Concat(inbound.Flights).ToList(),
                        TotalPrice = outbound.TotalPrice + inbound.TotalPrice
                    };
                    roundTripJourneys.Add(roundTripJourney);
                }
            }

            return new ApiResponseDto
            {
                Origin = origin,
                Destination = destination,
                Currency = currency,
                TotalRoutes = roundTripJourneys.Count,
                Journeys = roundTripJourneys
            };
        }


    }
}
