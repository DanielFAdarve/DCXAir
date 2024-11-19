namespace DCXAir.API.Application.DTOs
{
    public class JourneyDto
    {
        public string Type { get; set; }
        public List<FlightDto> Flights { get; set; } = new();
        public double TotalPrice { get; set; }    
    }
}
