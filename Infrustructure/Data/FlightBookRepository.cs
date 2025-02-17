using Domain.Commons.Contract; 
using Domain.Commons.Entities;
using Microsoft.EntityFrameworkCore; 
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Data
{
    public class FlightBookRepository : IFlightBookRepository
    {
        private readonly FlyDbContext _context;

        public FlightBookRepository(FlyDbContext context)
        {
            _context = context;
        }

        public async Task<List<FlightBook>> GetFlightBooksAsync()
        {
            return await _context.FlightBooks.ToListAsync();
        }

        public async Task<FlightBook> GetFlightBookByIdAsync(Guid id)
        {
            return await _context.FlightBooks.FindAsync(id);
        }

        public async Task AddFlightBookAsync(FlightBook flightBook)
        {
            await _context.FlightBooks.AddAsync(flightBook);

            await _context.SaveChangesAsync();
        }
        public async Task UpdateFlightBookAsync(FlightBook flightBook)
        {
            _context.FlightBooks.Update(flightBook);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFlightBookAsync(FlightBook flightBook)
        {
            _context.FlightBooks.Remove(flightBook);
            await _context.SaveChangesAsync();
        }

        public async Task<List<FlightBook>> GetActiveBookingsByUserAsync(Guid userId)
        {
            return await _context.FlightBooks
                .Where(b => b.UserId == userId && b.IsActive == true)
                .ToListAsync();
        }

        public async Task<List<FlightBook>> GetAllBookingsByUserAsync(Guid userId)
        {
            return await _context.FlightBooks
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }
    }
}
    
     
