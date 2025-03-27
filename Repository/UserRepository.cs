using Microsoft.EntityFrameworkCore;
using Movie.Models;
using Movie.RequestDTO;

namespace Movie.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly movieDB _context;

        public UserRepository(movieDB context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByUserNameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}