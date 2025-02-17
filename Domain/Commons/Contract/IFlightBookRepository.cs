using Domain.Commons.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IFlightBookRepository
{
    Task<List<FlightBook>> GetFlightBooksAsync();
    Task<FlightBook> GetFlightBookByIdAsync(Guid id);
    Task AddFlightBookAsync(FlightBook flightBook);
    Task UpdateFlightBookAsync(FlightBook flightBook);
    Task DeleteFlightBookAsync(FlightBook flightBook);
    Task<List<FlightBook>> GetActiveBookingsByUserAsync(Guid userId);
    Task<List<FlightBook>> GetAllBookingsByUserAsync(Guid userId);
}
