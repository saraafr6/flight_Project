using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Domain.Commons.Contract;
using Domain.Commons.Entities;
using Swashbuckle.AspNetCore.Annotations;

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
        [SwaggerOperation(Summary = "Get all flight bookings", Description = "Retrieves a list of all flight bookings along with their associated flight details.")]

        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _context.FlightBook.Include(b => b.Flight).ToListAsync();
            return Ok(bookings);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get booking by ID", Description = "Retrieves the details of a specific flight booking by its unique identifier.")]

        public async Task<IActionResult> GetBookingById(Guid id)
        {
            var booking = await _context.FlightBook
                .Include(b => b.Flight)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
                return NotFound("Booking not found.");

            return Ok(booking);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new flight booking", Description = "Creates a new flight booking if seats are available, and sends a confirmation email to the user.")]

        public async Task<IActionResult> CreateBooking([FromBody] FlightBook booking)
        {
            if (booking == null)
                return BadRequest("Booking data is required.");

            var flight = await _context.Flight.FindAsync(booking.FlightId);
            if (flight == null)
                return NotFound("Flight not found.");

            if (flight.AvailableSeats <= await _context.FlightBook.CountAsync(b => b.FlightId == flight.Id))
                return BadRequest("No seats available.");

            _context.FlightBook.Add(booking);
            await _context.SaveChangesAsync();

            await _emailService.SendEmailAsync("sararezaei563@gmail.com", flight.FlightNumber, flight.DepartureTime.ToString("yyyy-MM-dd HH:mm"));
            return CreatedAtAction(nameof(GetBookingById), new { id = booking.Id }, booking);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a booking", Description = "Deletes a specific flight booking by its unique identifier.")]

        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _context.FlightBook.FindAsync(id);
            if (booking == null)
                return NotFound("Booking not found.");

            _context.FlightBook.Remove(booking);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
