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

   
        public async Task<IEnumerable<RequestMovieDTO>> GetMoviesAsync(int pageNumber, int pageSize, string sortBy, string search)
        {
            var query = _context.Movies
                .Include(m => m.Director)
                .Include(m => m.MovieActors).ThenInclude(ma => ma.Actor)
                .Include(m => m.MovieCategories).ThenInclude(mc => mc.Category)
                .Where(m => m.Status == 1);

            // 🔎 Filtering
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(m => m.Title.Contains(search) || m.Description.Contains(search));
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
                DirectorName = m.Director?.NameDir
            }).ToList();
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
                DirectorName = movie.Director?.NameDir
            };
        }

        //public async Task<RequestMovieDTO> AddAsync(RequestMovieDTO movieDTO)
        //{
        //    var movie = new Movie
        //    {
        //        Title = movieDTO.Title,
        //        Description = movieDTO.Description,
        //        Rating = movieDTO.Rating,
        //        PosterUrl = movieDTO.PosterUrl,
        //        AvatarUrl = movieDTO.AvatarUrl,
        //        LinkFilmUrl = movieDTO.LinkFilmUrl,
        //        DirectorID = movieDTO.DirectorID,
        //        Status = 1
        //    };

        //    _context.Movies.Add(movie);
        //    await _context.SaveChangesAsync();

        //    return movieDTO;
        //}

        //public async Task UpdateAsync(Movie movie)
        //{
        //    _context.Movies.Update(movie);
        //    await _context.SaveChangesAsync();
        //}

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
            // Lấy danh sách poster của phim (3 poster)
            var posters = await _context.Movies
                .Select(x => new ResponseMovieDTO
                {
                    MovieId = x.MovieId,
                    Title = x.Title,
                    AvatarUrl = x.AvatarUrl,
                    PosterUrl = x.PosterUrl
                })
                .Where(y => y.PosterUrl != null)
                .Take(3)
                .ToListAsync();

            var hotMovies = await _context.Movies
                .Select(x=> new ResponseMovieDTO
                {
                    MovieId = x.MovieId,
                    Title = x.Title,
                    AvatarUrl = x.AvatarUrl,
                    PosterUrl = x.PosterUrl,
                    IsHot = x.IsHot
                })
                .Where(y => y.IsHot != null && y.IsHot == true)
                .Take(1)
                .ToListAsync();
                    
            // Lấy danh sách phim hành động (thể loại có ID = 1)
            var actionMovies = await _context.Movies
                .Where(x => x.MovieCategories.Select(y => y.CategoriesID).Contains(1))
                .Select(x => new ResponseMovieDTO
                {
                    MovieId = x.MovieId,
                    Title = x.Title,
                    Description = x.Description,
                    Rating = x.Rating,
                    PosterUrl = x.PosterUrl,
                    AvatarUrl = x.AvatarUrl,
                    LinkFilmUrl = x.LinkFilmUrl
                })
                .ToListAsync();


            var home = new HomeResponse
            {
                PosterMovies = posters,
                ActionMovies = actionMovies,
                HotMovies = hotMovies
            };

            return home;
        }

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

            await _context.SaveChangesAsync();
            return movieDTO;
        }

        //  Xoá mềm
        public async Task SoftDeleteAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                movie.Status = 0;
                await _context.SaveChangesAsync();
            }
        }

        //  Lịch sử xoá
        public async Task<IEnumerable<RequestMovieDTO>> GetDeletedMoviesAsync()
        {
            return await GetMoviesAsync(1, 100, "Title", "");
        }

        //  Xoá vĩnh viễn
        public async Task DeleteDeletedMoviesAsync()
        {
            var movies = _context.Movies.Where(m => m.Status == 0);
            _context.Movies.RemoveRange(movies);
            await _context.SaveChangesAsync();
        }


    }
}