using Domain.Commons.Contract; 
using Domain.Commons.Entities;
using Infrastructure.Data; 

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class FlightRepository : IFlightRepository
    {
        private readonly FlyDbContext _context; 

        public FlightRepository(FlyDbContext context)
        {
            _context = context;
        }

        public async Task<List<Flight>> GetFlightsAsync()
        {
            return await _context.Flight.ToListAsync();
        }
        public async Task<Flight> GetFlightByIdAsync(Guid id)
        {
            return await _context.Flight.FindAsync(id);
        }
        public async Task AddFlightAsync(Flight flight)
        {
            await _context.Flight.AddAsync(flight); 
            await _context.SaveChangesAsync(); 
        }

        public async Task UpdateFlightAsync(Flight flight)
        {
            _context.Flight.Update(flight);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFlightAsync(Flight flight)
        {
            _context.Flight.Remove(flight); 
            await _context.SaveChangesAsync(); 
        }
    }
}