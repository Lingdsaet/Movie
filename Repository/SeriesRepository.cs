using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Movie.Models;
using Movie.RequestDTO;
using Movie.ResponseDTO;

namespace Movie.Repository
{
    public class SeriesRepository : ISeriesRepository
    {
        private readonly movieDB _context;
        public SeriesRepository(movieDB context)
        {
            _context = context;
        }
       
        public async Task<RequestSeriesDTO> GetSeriesByIdAsync(int id)
        {

            var series = await _context.Series
            .Include(s => s.SeriesActors)
                .ThenInclude(sa => sa.Actors)
            .Include(s => s.SeriesCategories)
                .ThenInclude(sc => sc.Categories)
            .Include(s => s.Director)
            .FirstOrDefaultAsync(s => s.SeriesId == id);

            if (series == null) return null;

            var seriesDTO = new RequestSeriesDTO
            {
                Title = series.Title,
                YearReleased = series.YearReleased,
                Nation = series.Nation ?? string.Empty,
                Categories = series.SeriesCategories
                    .Select(sc => new RequestCategoryDTO
                    {
                        CategoryName = sc.Categories.CategoryName
                    }).ToList(),
                Description = series.Description ?? string.Empty,
                Episode = await _context.Episodes
                    .Where(e => e.SeriesId == series.SeriesId)
                    .Select(e => new RequestEpisodeDTO
                    {
                        EpisodeNumber = e.EpisodeNumber,
                        Title = e.Title ?? string.Empty,
                        LinkFilmUrl = e.LinkFilmUrl ?? string.Empty
                    }).ToListAsync(),
                TotalEpisode = series.Status ?? 0,
                Actors = series.SeriesActors.Select(sa => new RequestActorDTO
                {
                    ActorId = sa.ActorId,
                    NameAct = sa.Actors.NameAct
                }).ToList(),
                Director = series.Director?.NameDir ?? string.Empty
            };

            return seriesDTO;
        }

        public async Task<IEnumerable<RequestSeriesDTO>> GetSeriesAsync(int pageNumber, int pageSize, string sortBy, string search, int? categoryID)
        {
            var query = _context.Series
                .Where(s => s.Status == 1); 

            // Filtering
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s => s.Title.Contains(search));
            }

            if (categoryID.HasValue)
            {
                query = query.Where(s => s.SeriesCategories.Any(sc => sc.CategoryId == categoryID.Value));
            }

            // Sorting
            query = sortBy switch
            {
                "Title" => query.OrderBy(s => s.Title),
                "Rating" => query.OrderByDescending(s => s.Rating),
                _ => query.OrderBy(s => s.Title)
            };

            var seriesList = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return seriesList.Select(s => new RequestSeriesDTO
            {
                SeriesId = s.SeriesId,
                Title = s.Title,
                PosterUrl = s.PosterUrl,
                AvatarUrl = s.AvatarUrl,

               
            }).ToList();
        }

    }
}