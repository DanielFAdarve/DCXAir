namespace DCXAir.API.Application.Repositories
{
    using DCXAir.API.Domain.Entities;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IJourneyRepository
    {
        Task<List<Journey>> GetJourneysByOriginAndDestinationAsync(string origin, string destination);

        Task<List<Journey>> GetJourneysWithStopsAsync(string origin, string destination);
        Task<List<Journey>> GetAllJourneysAsync();
        Task AddJourneyAsync(Journey journey);
    }
}