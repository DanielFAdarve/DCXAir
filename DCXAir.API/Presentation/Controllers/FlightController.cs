namespace DCXAir.API.Presentation.Controllers
{
    using DCXAir.API.Application.DTOs;
    using DCXAir.API.Application.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class FlightController : ControllerBase
    {
        private readonly IJourneyService _journeyService;

        public FlightController(IJourneyService journey)
        {
            _journeyService = journey;
        }

        [HttpGet]
        public ActionResult Get(string origin, string destination, string type, string currency = "USD")
        {
            var journey = _journeyService.GetFligths(origin, destination, type, currency);
            return Ok(journey);

        }
    }
}
