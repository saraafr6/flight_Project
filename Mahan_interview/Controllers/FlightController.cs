using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> GetFlights()
        {
            var flights = await _context.Flight.ToListAsync();
            return Ok(flights);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFlightById(Guid id)
        {
            var flight = await _context.Flight
                .Include(f => f.Bookings) 
                .FirstOrDefaultAsync(f => f.Id == id);

            if (flight == null)
                return NotFound("Flight not found.");

            return Ok(flight);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFlight([FromBody] Flight flight)
        {
            if (flight == null)
                return BadRequest("Flight data is required.");

            _context.Flight.Add(flight);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetFlightById), new { id = flight.Id }, flight);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFlight(Guid id, [FromBody] Flight flight)
        {
            if (flight == null || id != flight.Id)
                return BadRequest("Invalid flight data.");

            var existingFlight = await _context.Flight.FindAsync(id);
            if (existingFlight == null)
                return NotFound("Flight not found.");

            
            existingFlight.AirlineName = flight.AirlineName;
            existingFlight.DepartureTime = flight.DepartureTime;
            existingFlight.ArrivalTime = flight.ArrivalTime;
            existingFlight.Price = flight.Price;
            existingFlight.AvailableSeats = flight.AvailableSeats;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlight(int id)
        {
            var flight = await _context.Flight.FindAsync(id);
            if (flight == null)
                return NotFound("Flight not found.");

            _context.Flight.Remove(flight);
            await _context.SaveChangesAsync();
            return NoContent();
        }

       
        [HttpGet("{id}/bookings")]
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
