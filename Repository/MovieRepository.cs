using Movies.Models;
using Microsoft.EntityFrameworkCore;

namespace Movies.Repository
{
    public class MovieRepository : IRepository<Movie>
    {
        private readonly movieDB _context;

        public MovieRepository(movieDB context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Movie>> GetAllAsync()
        {
            return await _context.Movies.Include(m => m.Categories).ToListAsync();
        }

        public async Task<Movie> GetByIdAsync(int id)
        {
            return await _context.Movies.Include(m => m.Categories).FirstOrDefaultAsync(m => m.MovieId == id);
        }

        public async Task AddAsync(Movie entity)
        {
            await _context.Movies.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Movie entity)
        {
            _context.Movies.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
            }
        }
    }
}
