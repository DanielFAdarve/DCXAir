namespace DCXAir.API.Application.Interfaces
{
    using DCXAir.API.Application.DTOs;
    public interface IJourneyService
    {
        List<JourneyDto> GetFligths(string origin, string destination,string type, string currency);
    }
}
