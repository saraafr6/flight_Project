using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Domain.Commons.Contract;
using Domain.Commons.Entities;

namespace Main.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightBookingController : ControllerBase
    {
        private readonly FlyDbContext _context;
        private readonly IEmailService _emailService;

        public FlightBookingController(FlyDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

       
        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _context.FlightBooks.Include(b => b.Flight).ToListAsync();
            return Ok(bookings);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(Guid id)
        {
            var booking = await _context.FlightBooks
                .Include(b => b.Flight)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
                return NotFound("Booking not found.");

            return Ok(booking);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] FlightBook booking)
        {
            if (booking == null)
                return BadRequest("Booking data is required.");

            var flight = await _context.Flight.FindAsync(booking.FlightId);
            if (flight == null)
                return NotFound("Flight not found.");

            if (flight.AvailableSeats <= await _context.FlightBooks.CountAsync(b => b.FlightId == flight.Id))
                return BadRequest("No seats available.");

            _context.FlightBooks.Add(booking);
            await _context.SaveChangesAsync();

            await _emailService.SendEmailAsync("user@example.com", flight.FlightNumber, flight.DepartureTime.ToString("yyyy-MM-dd HH:mm"));
            return CreatedAtAction(nameof(GetBookingById), new { id = booking.Id }, booking);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _context.FlightBooks.FindAsync(id);
            if (booking == null)
                return NotFound("Booking not found.");

            _context.FlightBooks.Remove(booking);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
