using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Domain.Commons.Entities;
using Domain.Commons.Contract;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;

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

        #region CreateBooking

        [HttpPost("{userId}/bookings/{flightId}")]
        [SwaggerOperation(Summary = "Create a flight booking for a user", Description = "Book a flight for the user if seats are available.")]
        public async Task<IActionResult> CreateBooking(Guid userId, Guid flightId)
        {
            var userIdFromToken = HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdFromToken == null || userIdFromToken != userId.ToString())
            {
                return Unauthorized("You are not authorized to perform this action.");
            }

            var flight = await _context.Flight.FindAsync(flightId);
            if (flight == null)
                return NotFound("Flight not found.");

            var user = await _context.User.FindAsync(userId);
            if (user == null)
                return NotFound("User not found.");

            var bookedSeats = await _context.FlightBook.CountAsync(b => b.FlightId == flightId);
            if (flight.AvailableSeats <= bookedSeats)
                return BadRequest("No seats available.");

            
            var booking = new FlightBook
            {
                FlightId = flightId,
                UserId = userId,
                BookingDate = DateTime.UtcNow,
                CreatedBy = user.UserName,
                TotalPrice = flight.Price,
                IsActive = true
            };

            _context.FlightBook.Add(booking);
            flight.AvailableSeats--; 
            await _context.SaveChangesAsync();

            var bookingDto = new FlightBookDto
            {
                Id = booking.Id,
                FlightId = booking.FlightId,
                FlightNumber = flight.FlightNumber,
                UserId = booking.UserId,
                UserName = user.UserName,
                BookingDate = booking.BookingDate,
                TotalPrice = booking.TotalPrice,
                IsActive = booking.IsActive,
                CreateDateTime = booking.CreateDateTime
            };

            return CreatedAtAction(nameof(GetBookingById), new { userId = userId, bookingId = booking.Id }, bookingDto);
        }

        #endregion

        #region GetBookingById

        [HttpGet("{userId}/bookings/{bookingId}")]
        [SwaggerOperation(Summary = "Get specific booking for a user", Description = "Retrieve a flight booking for the user by bookingId.")]
        public async Task<IActionResult> GetBookingById(Guid userId, Guid bookingId)
        {
            var booking = await _context.FlightBook
                .Where(b => b.UserId == userId && b.Id == bookingId)
                .Include(b => b.Flight)
                .FirstOrDefaultAsync();

            if (booking == null)
                return NotFound("Booking not found.");

            var bookingDto = new FlightBookDto
            {
                Id = booking.Id,
                FlightId = booking.FlightId,
                FlightNumber = booking.Flight.FlightNumber,
                UserId = booking.UserId,
                UserName = booking.User != null ? booking.User.UserName : "Unknown",
                BookingDate = booking.BookingDate,
                TotalPrice = booking.TotalPrice,
                IsActive = booking.IsActive,
                CreateDateTime = booking.CreateDateTime ?? DateTimeOffset.MinValue
            };

            return Ok(bookingDto);
        }

        #endregion



        #region GetUserBookingHistory

        [HttpGet("{userId}/bookings")]
        [SwaggerOperation(Summary = "Get booking history for a user", Description = "Retrieve all flight bookings for the user.")]
        public async Task<IActionResult> GetUserBookingHistory(Guid userId)
        {
            var bookings = await _context.FlightBook
                .Where(b => b.UserId == userId)  
                .Include(b => b.Flight)
                .Include(b => b.User)
                .OrderByDescending(b => b.BookingDate)  
                .ToListAsync();

            if (bookings == null || !bookings.Any())
                return NotFound("No bookings found for this user.");

            
            var bookingDtos = bookings.Select(b => new FlightBookDto
            {
                Id = b.Id,
                FlightId = b.FlightId,
                FlightNumber = b.Flight != null ? b.Flight.FlightNumber : "Unknown",
                UserId = b.UserId,
                UserName = b.User != null ? b.User.UserName : "Unknown",
                BookingDate = b.BookingDate,
                TotalPrice = b.TotalPrice,
                IsActive = b.IsActive,
                CreateDateTime = b.CreateDateTime ?? DateTimeOffset.MinValue
            }).ToList();

            return Ok(bookingDtos);
        }

        #endregion

        #region CancelBooking

        [HttpDelete("{userId}/bookings/{bookingId}")]
        [SwaggerOperation(Summary = "Cancel a flight booking for a user", Description = "Allows a user to cancel a flight booking.")]
        public async Task<IActionResult> CancelBooking(Guid userId, Guid bookingId)
        {
            var booking = await _context.FlightBook
                .Where(b => b.UserId == userId && b.Id == bookingId)
                .FirstOrDefaultAsync();

            if (booking == null)
                return NotFound("Booking not found.");

            // Restore available seats when a booking is canceled
            var flight = await _context.Flight.FindAsync(booking.FlightId);
            if (flight != null)
            {
                flight.AvailableSeats++;
            }

            _context.FlightBook.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        #endregion

        #region GetUserInfo

        [HttpGet("{userId}")]
        [SwaggerOperation(Summary = "Get user info")]
        public async Task<IActionResult> GetUserInfo(Guid userId)
        {
            var user = await _context.User
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound("User not found.");

            var userDto = new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                MobileNumber = user.MobileNumber,
                Email = user.Email,
                BirthDate = user.BirthDate,
                IsActive = user.IsActive
            };

            return Ok(userDto);
        }

        #endregion

        #region RegisterUser

        [HttpPost("register")]
        [SwaggerOperation(Summary = "Register a new user", Description = "Register a new user with their details.")]
        public async Task<IActionResult> RegisterUser([FromBody] UserDto userDto)
        {
            var existingUser = await _context.User
                .Where(u => u.Email == userDto.Email)
                .FirstOrDefaultAsync();

            if (existingUser != null)
                return BadRequest("User with this email already exists.");

            var newUser = new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                UserName = userDto.UserName,
                MobileNumber = userDto.MobileNumber,
                Email = userDto.Email,
                BirthDate = userDto.BirthDate,
                IsActive = true
            };

            _context.User.Add(newUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserInfo), new { userId = newUser.Id }, userDto);
        }

        #endregion
    }
}
