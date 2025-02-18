using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

using Domain.Commons.Entities;

namespace Main.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightController : ControllerBase
    {
        private readonly FlyDbContext _context;

        public FlightController(FlyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Retrieve all flights", Description = "Fetches a list of all flights available.")]
        public async Task<IActionResult> GetAllFlights()
        {
            var flights = await _context.Flight.ToListAsync();
            return Ok(flights);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get flight by ID", Description = "Fetches a flight by its unique identifier.")]

        public async Task<IActionResult> GetFlightById(Guid id)
        {
            var flight = await _context.Flight
                .Include(f => f.Bookings) 
                .FirstOrDefaultAsync(f => f.Id == id);

            if (flight == null)
                return NotFound("Flight not found.");

            return Ok(flight);
        }

        [HttpGet("{id}/bookings")]
        [SwaggerOperation(Summary = "Get flight bookings", Description = "Returns all bookings related to a specific flight.")]

        public async Task<IActionResult> GetFlightBookings(Guid id)
        {
            var flight = await _context.Flight
                .Include(f => f.Bookings)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (flight == null)
                return NotFound("Flight not found.");

            return Ok(flight.Bookings);
        }
    }
}
