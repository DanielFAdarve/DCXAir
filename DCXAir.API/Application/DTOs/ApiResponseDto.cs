namespace DCXAir.API.Application.DTOs
{
    public class ApiResponseDto
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string Currency { get; set; }
        public int TotalRoutes { get; set; }
        public List<JourneyDto> Journeys { get; set; } = new();
    }
}
