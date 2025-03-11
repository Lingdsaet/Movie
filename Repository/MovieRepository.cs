using Microsoft.EntityFrameworkCore;
using Movies.Models;
using Movies.RequestDTO;
using Movies.ResponseDTO;
using Nest;

namespace Movies.Repository
{

    public class MovieRepository : IMovieRepository
    {
        private readonly movieDB _context;

        public MovieRepository(movieDB context)
        {
            _context = context;
        }

        public async Task<RequestMovieDTO> AddAsync(RequestMovieDTO movieDTO)
        {
            var movie = new Movie
            {
                Title = movieDTO.Title,
                Description = movieDTO.Description,
                Rating = movieDTO.Rating,
                PosterUrl = movieDTO.PosterUrl,
                AvatarUrl = movieDTO.AvatarUrl,
                LinkFilmUrl = movieDTO.LinkFilmUrl,
                DirectorID = movieDTO.DirectorID,
                IsHot = movieDTO.IsHot,
                YearReleased = movieDTO.YearReleased,
                Status = 1
            };

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return movieDTO;
        }

        public async Task<RequestMovieDTO?> GetByIdAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null) return null;

            return new RequestMovieDTO
            {
                MovieId = movie.MovieId,
                Title = movie.Title,
                Description = movie.Description,
                Rating = movie.Rating,
                PosterUrl = movie.PosterUrl,
                AvatarUrl = movie.AvatarUrl,
                LinkFilmUrl = movie.LinkFilmUrl,
                DirectorID = movie.DirectorID,
                DirectorName = movie.Director?.NameDir,
                IsHot = movie.IsHot,
                YearReleased = movie.YearReleased

            };
        }

        // Sửa 
        public async Task<RequestMovieDTO?> UpdateAsync(RequestMovieDTO movieDTO)
        {
            var movie = await _context.Movies.FindAsync(movieDTO.MovieId);
            if (movie == null) return null;

            movie.Title = movieDTO.Title;
            movie.Description = movieDTO.Description;
            movie.Rating = movieDTO.Rating;
            movie.PosterUrl = movieDTO.PosterUrl;
            movie.AvatarUrl = movieDTO.AvatarUrl;
            movie.LinkFilmUrl = movieDTO.LinkFilmUrl;
            movie.DirectorID = movieDTO.DirectorID;
            movie.IsHot = movieDTO.IsHot;
            movie.YearReleased = movie.YearReleased;

            await _context.SaveChangesAsync();
            return movieDTO;
        }

        public async Task<IEnumerable<RequestMovieDTO>> GetMoviesAsync(int pageNumber, int pageSize, string sortBy, string search, int? categoryID)
        {
            var query = _context.Movies
                .Include(m => m.Director)
                .Include(m => m.MovieActor).ThenInclude(ma => ma.Actor)
                .Include(m => m.MovieCategory).ThenInclude(mc => mc.Category)
                .Where(m => m.Status == 1);

            //  Filtering
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(m => m.Title.Contains(search));
            }

            if (categoryID.HasValue)
            {
                query = query.Where(m => m.MovieCategory.Any(mc => mc.CategoriesID == categoryID.Value));
            }

            //  Sorting
            query = sortBy switch
            {
                "Title" => query.OrderBy(m => m.Title),
                "Rating" => query.OrderByDescending(m => m.Rating),
                _ => query.OrderBy(m => m.Title)
            };

            var movies = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return movies.Select(m => new RequestMovieDTO
            {
                MovieId = m.MovieId,
                Title = m.Title,
                Description = m.Description,
                Rating = m.Rating,
                PosterUrl = m.PosterUrl,
                AvatarUrl = m.AvatarUrl,
                LinkFilmUrl = m.LinkFilmUrl,
                DirectorID = m.DirectorID,
                DirectorName = m.Director?.NameDir,
                Actors = m.MovieActor.Select(ma => new RequestActorDTO
                 {
                     ActorsID = ma.Actor.ActorsID,
                    NameAct = ma.Actor.NameAct
                 }).ToList(),
                Categories = m.MovieCategory.Select(mc => new RequestCategoryDTO
                {
                    CategoriesID = mc.Category.CategoriesID,
                    CategoryName = mc.Category.CategoryName
                }).ToList()
            }).ToList();
        }
        //Xoá mềm 
        public async Task SoftDeleteAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                movie.Status = 0;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var movies = _context.Movies.Where(m => m.Status == 0);
            _context.Movies.RemoveRange(movies);
            await _context.SaveChangesAsync();

        }
    }
}