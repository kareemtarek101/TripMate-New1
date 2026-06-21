using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/flights")]
public class FlightsController : ControllerBase
{
    private readonly ISerpApiService _serpService;

    public FlightsController(ISerpApiService serpService)
    {
        _serpService = serpService;
    }

    [HttpGet]
    public async Task<IActionResult> Get(string from, string to)
    {
        var result = await _serpService.GetFlights(
            from,
            to,
            DateOnly.FromDateTime(DateTime.UtcNow.AddDays(7))
        );

        return Ok(new ApiResponse<List<FlightDto>>(result));
    }
}