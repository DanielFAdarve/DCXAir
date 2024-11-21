namespace DCXAir.API.Presentation.Controllers
{
    using DCXAir.API.Application.DTOs;
    using DCXAir.API.Application.Interfaces;
    using DCXAir.API.Application.Repositories;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    [ApiController]
    [Route("api/[controller]")]
    public class FlightController : ControllerBase
    {
        private readonly IJourneyService _journeyService;
        private readonly IJourneyRepository _journeyRepository;

        private readonly IMemoryCache _cache;

        public FlightController(IJourneyService journey, IMemoryCache cache, IJourneyRepository journeyRepository)
        {
            _journeyService = journey;
            _journeyRepository = journeyRepository;
            _cache = cache;
        }

        [HttpGet]

        public async Task<IActionResult> GetFligths(string origin, string destination, string type, string currency = "USD")
        {
            var cacheKey = $"{origin}-{destination}-{type}-{currency}";
            if (_cache.TryGetValue(cacheKey, out IEnumerable<JourneyDto> cachedJourneys))
            {
                Console.WriteLine("Data taked from memory");
                return Ok(cachedJourneys);
            }

            var journeys = await _journeyService.GetFligths(origin, destination, type, currency);

            var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromSeconds(45))
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(60))
            .SetPriority(CacheItemPriority.Normal);

            _cache.Set(cacheKey, journeys, cacheEntryOptions);
            Console.WriteLine("Data load in memory");
            return Ok(journeys);

        }

        [HttpGet("check-data")]
        public async Task<IActionResult> CheckData()
        {
            var journeys = await _journeyRepository.GetAllJourneysAsync();

            if (journeys.Count == 0)
            {
                return NotFound("No Journeys in DB.");
            }

            return Ok($"There are {journeys.Count} journeys in DB.");
        }
    }
}