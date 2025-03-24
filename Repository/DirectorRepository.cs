using Microsoft.EntityFrameworkCore;
using Movie.Models;
using Movie.RequestDTO;

namespace Movie.Repository
{
    public class DirectorRepository : IDirectorsRepository
    {
        private readonly movieDB _context;

        public DirectorRepository(movieDB context)
        {
            _context = context;
        }

        public async Task<DirectorDetailDTO?> GetDirectorByIdAsync(int id)
        {
            var director = await _context.Directors
             .Include(d => d.Movie)
             .Include(d => d.Series)
             .FirstOrDefaultAsync(d => d.DirectorId == id);

            if (director == null) return null;

            var directorDetail = new DirectorDetailDTO
            {
                Director = new RequestDirectorDTO
                {
                    DirectorId = director.DirectorId,
                    NameDir = director.NameDir,
                    Nationality = director.Nationality
                },
                Movies = director.Movie.Select(m => new DirectorMoviesDTO
                {
                    MovieId = m.MovieId,
                    AvatarUrl = m.AvatarUrl,
                    MovieName = m.Title
                }).ToList(),
                Series = director.Series.Select(ma => new DirectorMoviesDTO
                {
                    MovieId = ma.SeriesId,
                    AvatarUrl = ma.AvatarUrl,
                    MovieName = ma.Title
                }).ToList()

            };

            return directorDetail;
        }
    }
}
