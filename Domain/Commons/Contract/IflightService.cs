
using Domain.Commons.Entities;

namespace Domain.Commons.Contract
{
    public interface IFlightService
    {
        Task<List<Flight>> GetFlightsAsync();
        Task<List<Flight>> GetFlightsByDestinationAndDateAsync(string destination, DateTime date);
        Task BookFlightAsync(Guid flightId, Guid userId, decimal totalPrice);
        Task<List<FlightBook>> GetActiveBookingsByUserAsync(Guid userId);
        Task<List<FlightBook>> GetAllBookingsByUserAsync(Guid userId);
        Task CancelBookingAsync(Guid bookingId);
    }
}
