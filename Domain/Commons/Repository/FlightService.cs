using Domain.Commons.Contract; 
using Domain.Commons.Entities;


using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Commons;
public class FlightService : IFlightService
{
    private readonly IFlightRepository _flightRepository;
    private readonly IFlightBookRepository _flightBookRepository;
    private readonly IEmailService _emailService;

    public FlightService(IFlightRepository flightRepository, IFlightBookRepository flightBookRepository, IEmailService emailService)
    {
        _flightRepository = flightRepository;
        _flightBookRepository = flightBookRepository;
        _emailService = emailService;
    }

    public async Task<List<Flight>> GetFlightsAsync()
    {
        return await _flightRepository.GetFlightsAsync();
    }

    public async Task<Flight> GetFlightByIdAsync(Guid id)
    {
        return await _flightRepository.GetFlightByIdAsync(id);
    }

    public async Task<List<Flight>> GetFlightsByDestinationAndDateAsync(string destination, DateTime date)
    {
        return await _flightRepository.GetFlightsAsync();
    }

    public async Task BookFlightAsync(Guid flightId, Guid userId, decimal totalPrice)
    {
        var flight = await _flightRepository.GetFlightByIdAsync(flightId);

        if (flight != null && flight.AvailableSeats > 0)
        {

            var flightBooking = new FlightBook
            {
                FlightId = flightId,
                UserId = userId,
                BookingDate = DateTimeOffset.Now,
                TotalPrice = totalPrice,
                IsActive = true
            };


            flight.AvailableSeats--;


            await _flightBookRepository.AddFlightBookAsync(flightBooking);
            await _flightRepository.UpdateFlightAsync(flight);


            var userEmail = "user@example.com";
            await _emailService.SendEmailAsync(userEmail, "Confirmation of Your Flight Booking",
                $"Your flight with flight number {flight.FlightNumber} is successfully booked. Total price: {totalPrice}");
        }
        else
        {
            throw new Exception("Flight not available or no seats left.");
        }
    }


    public async Task<List<FlightBook>> GetActiveBookingsByUserAsync(Guid userId)
    {
        return await _flightBookRepository.GetActiveBookingsByUserAsync(userId);
    }

    public async Task<List<FlightBook>> GetAllBookingsByUserAsync(Guid userId)
    {
        return await _flightBookRepository.GetAllBookingsByUserAsync(userId);
    }

    public async Task CancelBookingAsync(Guid bookingId)
    {
        var booking = await _flightBookRepository.GetFlightBookByIdAsync(bookingId);

        if (booking != null && booking.IsActive)
        {
            booking.IsActive = false;
            var flight = await _flightRepository.GetFlightByIdAsync(booking.FlightId);

            if (flight != null)
            {
                flight.AvailableSeats++;
                await _flightRepository.UpdateFlightAsync(flight);
            }
            await _flightBookRepository.UpdateFlightBookAsync(booking);
        }
        else
        {
            throw new Exception("Booking not found or already cancelled.");
        }
    }

}

