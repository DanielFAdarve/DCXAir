namespace DCXAir.API.Presentation.Controllers
{
    using DCXAir.API.Application.DTOs;
    using DCXAir.API.Application.Interfaces;
    using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    [ApiController]
    [Route("api/[controller]")]
    public class FlightController : ControllerBase
    {
        private readonly IJourneyService _journeyService;
        private readonly IMemoryCache _cache;

        public FlightController(IJourneyService journey, IMemoryCache cache)
        {
            _journeyService = journey;
            _cache = cache;
        }

        [HttpGet]
        public ActionResult Get(string origin, string destination, string type, string currency = "USD")
        {
            var cacheKey = $"{origin}-{destination}-{type}-{currency}";
            if (_cache.TryGetValue(cacheKey, out IEnumerable<JourneyDto> cachedJourneys)) {
                Console.WriteLine("Data taked from memory");
                return Ok(cachedJourneys);
            }

            var journey = _journeyService.GetFligths(origin, destination, type, currency);

            var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromSeconds(45))
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(60))
            .SetPriority(CacheItemPriority.Normal);

            _cache.Set(cacheKey, journey, cacheEntryOptions);
            Console.WriteLine("Data load in memory");
            return Ok(journey);

        }
    }
}
