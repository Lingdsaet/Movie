using Microsoft.EntityFrameworkCore;
using Movies.Models;
using Movies.RequestDTO;
using Movies.ResponseDTO;

namespace Movies.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly movieDB _context;

        public MovieRepository(movieDB context)
        {
            _context = context;
        }

        //public async Task<IEnumerable<Movie>> GetAllAsync()
        //{
        //    return await _context.Movies.ToListAsync();
        //}

        public async Task<Movie> GetByIdAsync(int id)
        {
            return await _context.Movies.FindAsync(id);
        }

        public async Task AddAsync(Movie movie)
        {
            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Movie movie)
        {
            _context.Movies.Update(movie);
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

        public async Task<IEnumerable<RequestMovieDTO>> GetAllAsync()
        {
            var movies = await _context.Movies.ToListAsync();
            return movies.Select(movie => new RequestMovieDTO
            {
                MovieId = movie.MovieId,
                Title = movie.Title,
                Description = movie.Description,
                Rating = movie.Rating,
                PosterUrl = movie.PosterUrl,
                AvatarUrl = movie.AvatarUrl,
                LinkFilmUrl = movie.LinkFilmUrl
            }).ToList();
        }

        public async Task<HomeResponse> GetHomeMovies()
        {
            var posters = _context.Movies.Select(x => new ResponseMovieDTO
            {
                MovieId = x.MovieId,
                Title = x.Title,
                AvatarUrl= x.AvatarUrl
            }).Where(y => y.PosterUrl != null).Take(3);
            var actionMovies = _context.Movies.Select(x => new
            {
                x.MovieId,
                Categories = x.MovieCategories.Select(y => y.CategoriesID == 1).ToList()
            }).ToList();
            var home = new HomeResponse
            {
                PosterMovies = posters,
                ActionMovies = (IEnumerable<ResponseMovieDTO>)actionMovies
            };
            var movies = await _context.Movies.ToListAsync();
            movies.Select(movie => new ResponseMovieDTO
            {
                MovieId = movie.MovieId,
                Title = movie.Title,
                Description = movie.Description,
                Rating = movie.Rating,
                PosterUrl = movie.PosterUrl,
                AvatarUrl = movie.AvatarUrl,
                LinkFilmUrl = movie.LinkFilmUrl
            }).ToList();
        }

        public async Task AddAsync(RequestMovieDTO movieDTO)
        {
            var newMovie = new Movie
            {
                Title = movieDTO.Title,
                Description = movieDTO.Description,
                Rating = movieDTO.Rating,
                PosterUrl = movieDTO.PosterUrl,
                AvatarUrl = movieDTO.AvatarUrl,
                LinkFilmUrl = movieDTO.LinkFilmUrl
            };

            await _context.Movies.AddAsync(newMovie);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateAsync(RequestMovieDTO movieDTO)
        {
            var movie = await _context.Movies.FindAsync(movieDTO.MovieId);
            if (movie == null)
            {
                return;
            }

            movie.Title = movieDTO.Title;
            movie.Description = movieDTO.Description;
            movie.Rating = movieDTO.Rating;
            movie.PosterUrl = movieDTO.PosterUrl;
            movie.AvatarUrl = movieDTO.AvatarUrl;
            movie.LinkFilmUrl = movieDTO.LinkFilmUrl;

            _context.Movies.Update(movie);
            await _context.SaveChangesAsync();
        }

        Task<RequestMovieDTO> IMovieRepository.GetByIdAsync(int id)
        {
          
        }
    }
}