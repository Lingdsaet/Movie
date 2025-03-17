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
        public async Task<IEnumerable<Series>> GetAllAsync()
        {
            return await _context.Series.ToListAsync();
        }

        public Task<Series> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(Series entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Series entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
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
                LinkFilmUrl = series.LinkFilmUrl ?? string.Empty,
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
                    ActorsId = sa.ActorsId,
                    NameAct = sa.Actors.NameAct
                }).ToList(),
                Director = series.Director?.NameDir ?? string.Empty
            };

            return seriesDTO;
        }

        //// Lấy danh sách các tập phim theo Series
        //public async Task<List<RequestEpisodeDTO>> GetEpisodesBySeriesIdAsync(int seriesId)
        //{
        //    var episodes = await _context.Episodes
        //        .Where(e => e.SeriesId == seriesId)
        //        .Select(e => new RequestEpisodeDTO
        //        {
        //            EpisodeNumber = e.EpisodeNumber,
        //            Title = e.Title ?? "",
        //            LinkFilmUrl = e.LinkFilmUrl
        //        }).ToListAsync();

        //    return episodes;
        //}
    }
}