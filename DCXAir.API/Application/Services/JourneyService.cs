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

        public List<JourneyDto> GetFligths(string origin, string destination, string type, string currency)
        {
            if (type == "one-way")
            {
                return GetOneWayFlights(origin, destination, type, currency);
            }
            else if (type == "round-trip")
            {
                return GetRoundTripFlights(origin, destination, type, currency);
            }
            else
            {
                return new List<JourneyDto>();
            }
        }
        

       
        public List<JourneyDto> GetOneWayFlights(string origin, string destination,string type, string currency)
        {
            var journeys = new List<Journey>();

          
            var directJourneys = _journeys.Where(j => j.Fligths.Any(f => f.Origin == origin && f.Destination == destination)).ToList();
            journeys.AddRange(directJourneys);

      
            var connectingJourneys = GetFlightsWithStops(origin, destination, currency);
            journeys.AddRange(connectingJourneys);

    
            return journeys.Select(j => new JourneyDto
            {
                Type = j.Fligths.Count == 1 ? "One Way" : "With Stops", 
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
            }).ToList();
        }

        public List<Journey> GetFlightsWithStops(string origin, string destination, string currency)
        {
            var journeys = new List<Journey>();

            var directJourneys = _journeys.Where(j => j.Fligths.Any(f => f.Origin == origin && f.Destination == destination)).ToList();
            journeys.AddRange(directJourneys);


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

        public List<JourneyDto> GetRoundTripFlights(string origin, string destination,string type, string currency)
        {
 
            var outboundJourneys = GetOneWayFlights(origin, destination,type, currency);

            var returnJourneys = GetOneWayFlights(destination, origin,type, currency);

            var roundTripJourneys = new List<JourneyDto>();

            foreach (var outbound in outboundJourneys)
            {
                foreach (var inbound in returnJourneys)
                {
                    var roundTripJourney = new JourneyDto
                    {
                        Flights = outbound.Flights.Concat(inbound.Flights).ToList(),
                        TotalPrice = outbound.TotalPrice + inbound.TotalPrice 
                    };
                    roundTripJourneys.Add(roundTripJourney);
                }
            }

            return roundTripJourneys;
        }

        
    }
}
