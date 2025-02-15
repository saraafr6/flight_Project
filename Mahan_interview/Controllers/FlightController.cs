using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class FlightController : ControllerBase
{
    private readonly FlyDbContext _context;

    public FlightController(FlyDbContext context)
    {
        _context = context;
    }

    [HttpGet("available")]
    public async Task<IActionResult> GetAvailableFlights()
    {
        var flights = await _context.Flights
            .Where(f => f.AvailableSeats > 0)
            .Select(f => new FlightInfoDto
            {
                FlightNumber = f.FlightNumber,
                AirlineName = f.AirlineName,
                Origin = f.Origin,
                Destination = f.Destination,
                DepartureTime = f.DepartureTime,
                ArrivalTime = f.ArrivalTime,
                Price = f.Price
            }).ToListAsync();

        return Ok(flights);
    }
}
