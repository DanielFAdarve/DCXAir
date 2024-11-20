namespace DCXAir.API.Application.Interfaces
{
    using DCXAir.API.Application.DTOs;
    public interface IJourneyService
    {

        Task<List<JourneyDto>> GetOneWayFlights(string origin, string destination, string type, string currency);
        Task<ApiResponseDto> GetFligths(string origin, string destination, string type, string currency);
        Task<List<JourneyDto>> GetRoundTripFlights(string origin, string destination, string type, string currency);
    
    }
}
