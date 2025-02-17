using Domain.Commons.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IFlightRepository
{
    Task<List<Flight>> GetFlightsAsync();
    Task<Flight> GetFlightByIdAsync(Guid id);
    Task AddFlightAsync(Flight flight);
    Task UpdateFlightAsync(Flight flight);
    Task DeleteFlightAsync(Flight flight);
}
