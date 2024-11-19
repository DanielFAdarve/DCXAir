namespace DCXAir.API.Application.DTOs
{
    public class FlightDto
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
        public double Price { get; set; }
        public string Currency { get; set; }
        public List<TransportDto> Transport { get; set; } = new();
    }
}
