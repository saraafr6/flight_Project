using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

using Domain.Commons.Entities;
using Domain.Commons.Contract;

namespace Main.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly FlyDbContext _context;
        private readonly IEmailService _emailService;

        public UserController(FlyDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }
        [HttpPost("{userId}/bookings/{flightId}")]
        [SwaggerOperation(Summary = "Create a flight booking for a user", Description = "Book a flight for the user if seats are available.")]
        public async Task<IActionResult> CreateBooking(Guid userId, Guid flightId)
        {
           
            var flight = await _context.Flight.FindAsync(flightId);
            if (flight == null)
                return NotFound("Flight not found.");

            
            var bookedSeats = await _context.FlightBook.CountAsync(b => b.FlightId == flightId);
            if (flight.AvailableSeats <= bookedSeats)
                return BadRequest("No seats available.");

        
            var booking = new FlightBook
            {
                FlightId = flightId,
                UserId = userId,
                BookingDate = DateTime.UtcNow
            };

            _context.FlightBook.Add(booking);
            await _context.SaveChangesAsync();

            
            await _emailService.SendEmailAsync("user@example.com", flight.FlightNumber, flight.DepartureTime.ToString("yyyy-MM-dd HH:mm"));

            return CreatedAtAction(nameof(GetBookingById), new { userId = userId, bookingId = booking.Id }, booking);
        }

     
        [HttpGet("{userId}/bookings/{bookingId}")]
        public async Task<IActionResult> GetBookingById(Guid userId, Guid bookingId)
        {
            var booking = await _context.FlightBook
                .Where(b => b.UserId == userId && b.Id == bookingId)
                .Include(b => b.Flight)
                .FirstOrDefaultAsync();

            if (booking == null)
                return NotFound("Booking not found.");

            return Ok(booking);
        }
    

    [HttpDelete("{userId}/bookings/{bookingId}")]
        [SwaggerOperation(Summary = "Cancel a flight booking for a user", Description = "Allows a user to cancel a flight booking.")]
        public async Task<IActionResult> CancelBooking(Guid userId, Guid bookingId)
        {
            var booking = await _context.FlightBook
                .Where(b => b.UserId == userId && b.Id == bookingId)
                .FirstOrDefaultAsync();

            if (booking == null)
                return NotFound("Booking not found.");

            _context.FlightBook.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
