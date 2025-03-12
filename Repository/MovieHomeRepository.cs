using Movies.ResponseDTO;

namespace Movies.Repository
{
    public class MovieHomeRepository
    {
        // home
        public async Task<IEnumerable<string>> GetPostersAsync()
        {
            var posters = await _context.Movies
                .Where(m => m.Status == 1 && m.PosterUrl != null)
                .OrderByDescending(m => m.YearReleased)
                .Take(3)
                .Select(m => m.PosterUrl) // Chỉ lấy URL của poster
                .ToListAsync();

            return posters;
        }
        public async Task<IEnumerable<ResponseMovieDTO>> GetNewMoviesAsync()
        {
            var query = _context.Movies
                .Where(m => m.Status == 1)
                .OrderByDescending(m => m.YearReleased)
                .Take(10);

            return await query.Select(movie => new ResponseMovieDTO
            {
                MovieId = movie.MovieId,
                Title = movie.Title,
                AvatarUrl = movie.AvatarUrl,
            }).ToListAsync();
        }

        public async Task<IEnumerable<ResponseMovieDTO>> GetHotMoviesAsync()
        {
            var query = _context.Movies
                .Where(m => m.Status == 1 && m.IsHot == true)
                .OrderByDescending(m => m.Rating)
                .Take(10);

            return await query.Select(movie => new ResponseMovieDTO
            {
                MovieId = movie.MovieId,
                Title = movie.Title,
                AvatarUrl = movie.AvatarUrl,
            }).ToListAsync();
        }

        public async Task<IEnumerable<ResponseMovieDTO>> GetSeriesMoviesAsync()
        {
            var query = _context.Series
                .Where(s => s.Status == 1)
                .OrderByDescending(s => s.YearReleased)
                .Take(10);

            return await query.Select(series => new ResponseMovieDTO
            {
                MovieId = series.SeriesId,
                Title = series.Title,
                Description = series.Description,
                PosterUrl = series.PosterUrl,
                AvatarUrl = series.AvatarUrl,
                LinkFilmUrl = series.LinkFilmUrl
            }).ToListAsync();
            throw new NotImplementedException();
        }


        public async Task<IEnumerable<ResponseMovieDTO>> GetActionMoviesAsync()
        {
            var query = _context.Movies
                .Where(m => m.Status == 1 && m.MovieCategory.Any(mc => mc.Category.CategoryName == "Hành Động"))
                .OrderByDescending(m => m.YearReleased)
                .Take(10);

            return await query.Select(movie => new ResponseMovieDTO
            {
                MovieId = movie.MovieId,
                Title = movie.Title,
                AvatarUrl = movie.AvatarUrl,
            }).ToListAsync();
        }
    }
