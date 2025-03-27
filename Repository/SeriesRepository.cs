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

            if (series == null) return null!;

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
                    ActorId = sa.ActorId,
                    NameAct = sa.Actors.NameAct
                }).ToList(),
                Director = series.Director?.NameDir ?? string.Empty
            };

            return seriesDTO;
        }

        public async Task<IEnumerable<RequestMovieDTO>> GetMovieAsync(int pageNumber, int pageSize, string sortBy, string search, int? categoryID)
        {
            var query = _context.Movies
                .Include(m => m.Director)
                .Include(m => m.MovieActor).ThenInclude(ma => ma.Actors)
                .Include(m => m.MovieCategories).ThenInclude(mc => mc.Categories)
                .Where(m => m.Status == 1);

            //  Filtering
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(m => m.Title.Contains(search));
            }

            if (categoryID.HasValue)
            {
                query = query.Where(m => m.MovieCategories.Any(mc => mc.CategoryId == categoryID.Value));
            }

            //  Sorting
            query = sortBy switch
            {
                "Title" => query.OrderBy(m => m.Title),
                "Rating" => query.OrderByDescending(m => m.Rating),
                _ => query.OrderBy(m => m.Title)
            };

            var Movie = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Movie.Select(m => new RequestMovieDTO
            {
                MovieId = m.MovieId,
                Title = m.Title,
                Description = m.Description,
                Rating = m.Rating,
                PosterUrl = m.PosterUrl,
                AvatarUrl = m.AvatarUrl,
                LinkFilmUrl = m.LinkFilmUrl,
                DirectorId = m.DirectorId,
                Director = m.Director?.NameDir,
                Actors = m.MovieActor.Select(ma => new RequestActorDTO
                {
                    ActorId = ma.Actors.ActorId,
                    NameAct = ma.Actors.NameAct
                }).ToList(),
                Categories = m.MovieCategories.Select(mc => new RequestCategoryDTO
                {
                    CategoryId = mc.Categories.CategoryId,
                    CategoryName = mc.Categories.CategoryName
                }).ToList()
            }).ToList();
        }
    }
}