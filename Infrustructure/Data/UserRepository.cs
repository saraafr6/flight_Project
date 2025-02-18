using Domain.Commons.Contract; 
using Domain.Commons.Entities;
using Infrastructure.Data;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly FlyDbContext _context;

        public UserRepository(FlyDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.User
                .FirstOrDefaultAsync(u => u.UserName == username);  
        }
    }
}