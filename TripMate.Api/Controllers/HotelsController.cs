using Microsoft.AspNetCore.Mvc;
using TripMate.Application;

namespace TripMate.Api.Controllers
{
    [ApiController]
    [Route("api/hotels")]
    public class HotelsController : ControllerBase
    {
        private readonly ISerpApiService _serpService;

        public HotelsController(ISerpApiService serpService)
        {
            _serpService = serpService;
        }

        //  Get Hotels by City
        [HttpGet]
        public async Task<IActionResult> Get(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                return BadRequest(new ApiResponse<string>("City is required"));

            var result = await _serpService.GetHotels(city);

            return Ok(new ApiResponse<List<HotelDto>>(result));
        }
    }
}