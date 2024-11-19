namespace DCXAir.API.Application.Interfaces
{
    using DCXAir.API.Application.DTOs;
    public interface IJourneyService
    {
        Task<ApiResponseDto> GetFligths(string origin, string destination,string type, string currency);
        Task<ApiResponseDto> GetOneWayFlights(string origin, string destination, string type, string currency);
        Task<ApiResponseDto> GetRoundTripFlights(string origin, string destination,string type, string currency);
    }
}
