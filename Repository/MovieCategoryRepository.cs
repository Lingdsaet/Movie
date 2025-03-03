using Movies.Models;

namespace Movies.Repository
{
    public class MovieCategoryRepository
    {
        private readonly movieDB _context;
        public MovieCategoryRepository(movieDB context)
        {
            _context = context;
        }
    public async Task AddAsync(MovieRepository entity)
        {
            await _context.Movies.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
    public async Task DeleteByMovieIdAsync( int movieId)
        {

        }
    }
}
