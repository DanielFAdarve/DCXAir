namespace DCXAir.API.Application.Interfaces
{
    using DCXAir.API.Application.DTOs;
    public interface IJourneyService
    {
        ApiResponseDto GetFligths(string origin, string destination,string type, string currency);
        ApiResponseDto GetOneWayFlights(string origin, string destination, string type, string currency);
        ApiResponseDto GetRoundTripFlights(string origin, string destination,string type, string currency);
    }
}
