namespace DCXAir.API.Application.Interfaces
{
    using DCXAir.API.Domain.Entities;
    public interface IJourneyDataLoader
    {
        List<Flight> LoadFlights(string filePath);
    }
}