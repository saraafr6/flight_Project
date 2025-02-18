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


    }
}
