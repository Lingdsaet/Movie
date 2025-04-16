
using Microsoft.EntityFrameworkCore;
using Movie.Models;
using static Movie.RequestDTO.RequestContentDTO;

namespace Movie.Repository
{
    public class ContentRepository
    {
        private readonly movieDB _context;

        public ContentRepository(movieDB context)
        {
            _context = context;
        }
        public async Task<List<ActionContentDto>> GetActionContentAsync(
            string sortBy,
            bool isDescending,
            string search
)
        {
            //  Movie hành động
            var movieQuery = _context.Movies
                .Include(m => m.MovieCategories)
                    .ThenInclude(mc => mc.Categories)
                .Where(m => m.MovieCategories.Any(mc => mc.Categories.CategoryName == "Hành động"));

            

            //  Lọc theo tên
            if (!string.IsNullOrEmpty(search))
            {
                movieQuery = movieQuery.Where(m => m.Title.Contains(search));
               
            }

            //  Map về DTO
            var movieList = await movieQuery.Select(m => new ActionContentDto
            {
                MovieId = m.MovieId,
                Title = m.Title,
                AvatarUrl = m.AvatarUrl,
  
            }).ToListAsync();    

            return movieList

                .ToList();
        }

        public async Task<List<ActionContentDto>> GetHorrorContentAsync(
            string sortBy,
            bool isDescending,
            string search
)
        {
            //  Movie hành động
            var horrorQuery = _context.Movies
                .Include(m => m.MovieCategories)
                    .ThenInclude(mc => mc.Categories)
                .Where(m => m.MovieCategories.Any(mc => mc.Categories.CategoryName == "Kinh dị"));

            //  Lọc theo tên
            if (!string.IsNullOrEmpty(search))
            {
                horrorQuery = horrorQuery.Where(m => m.Title.Contains(search));
                
            }

            //  Map về DTO
            var movieList = await horrorQuery.Select(m => new ActionContentDto
            {
                MovieId = m.MovieId,
                Title = m.Title,
                AvatarUrl = m.AvatarUrl,
            }).ToListAsync();


            return movieList

                .ToList();
        }
        public async Task<List<AnimeContentDto>> GetAnimeContentAsync(
            string sortBy,
            bool isDescending,
            string search
)
        {
            //  Movie hành động
            var animeQuery = _context.Series
                .Include(m => m.SeriesCategories)
                    .ThenInclude(mc => mc.Categories)
                .Where(m => m.SeriesCategories.Any(mc => mc.Categories.CategoryName == "Anime"));

            //Lọc theo tên
            if (!string.IsNullOrEmpty(search))
            {
                animeQuery = animeQuery.Where(m => m.Title.Contains(search));

            }

            //  Map về DTO
            var animeList = await animeQuery.Select(m => new AnimeContentDto
            {
                SeriesId = m.SeriesId,
                Title = m.Title,
                AvatarUrl = m.AvatarUrl,
            }).ToListAsync();


            return animeList
                .ToList();

        }
    }
}
